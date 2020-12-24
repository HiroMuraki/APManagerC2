using APMControl.Interface;
using APMCore;
using APMCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using static APMControl.APM;

namespace APMControl {
    using PairCollection = DispatchedObservableCollection<IPair>;
    using WorkPairCollection = List<IPair>;

    public sealed class Container : ContainerBase, IContainer {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 过滤器
        /// </summary>
        public IFilter Filter {
            get {
                return _filter;
            }
            set {
                _filter = value;
                if (value != null) {
                    FilterUID = (value as Filter).FilterUID;
                }
                OnPropertyChanged(nameof(Filter));
            }
        }
        /// <summary>
        /// Pair组
        /// </summary>
        public PairCollection Pairs {
            get {
                return _pairs;
            }
        }
        /// <summary>
        /// 呈现透明度
        /// </summary>
        public double Opacity {
            get {
                return _opacity;
            }
            set {
                if (value < 0) {
                    _opacity = 0;
                } else if (value > 1) {
                    _opacity = 1;
                } else {
                    _opacity = value;
                }
                OnPropertyChanged(nameof(Opacity));
            }
        }
        /// <summary>
        /// 判断是否为空容器
        /// </summary>
        public bool IsEmpty {
            get {
                return CountPairs() == 0 && Header == "" && Description == "";
            }
        }
        #endregion

        #region 私有字段
        private readonly WorkPairCollection _workPairs;
        private readonly object _pairsLocker;
        #endregion

        #region 后备字段
        private double _opacity;
        private IFilter _filter;
        private readonly PairCollection _pairs;
        #endregion
        #endregion

        #region 构造函数
        public Container(APMCore.Model.Container source) : base(source) {
            _pairsLocker = new object();
            _workPairs = new WorkPairCollection();
            _pairs = new PairCollection();
            _opacity = 1;
        }
        #endregion

        #region 方法
        #region 公共方法
        /// <summary>
        /// 打开容器
        /// </summary>
        public async Task OpenAsync() {
            await Task.Run(() => {
                lock (_pairsLocker) {
                    ReloadPairs();
                }
            });
        }
        /// <summary>
        /// 关闭容器
        /// </summary>
        public async Task CloseAsync() {
            await Task.Run(() => {
                lock (_pairsLocker) {
                    UnloadPairs();
                }
            });
        }
        /// <summary>
        /// 更新数据至源
        /// </summary>
        /// <returns></returns>
        public override UpdateInformation UpdateToSource(SQLiteConnection conn, UpdateMethod updateMethod) {
            UpdateInformation updateInformation = base.UpdateToSource(conn, updateMethod);
            if (updateInformation.UpdateMethod == UpdateMethod.Delete) {
                foreach (Pair pair in FetchPairs((p) => true)) {
                    pair.UpdateToSource(conn, UpdateMethod.Delete);
                }
                if (File.Exists($@"{DataAvatarsFolderName}\{Avatar}")) {
                    File.Delete($@"{DataAvatarsFolderName}\{Avatar}");
                }
            } else {
                foreach (Pair pair in _workPairs) {
                    pair.UpdateToSource();
                }
            }
            return updateInformation;
        }
        public async Task<UpdateInformation> UpdateToSourceAsync() {
            return await Task.Run(() => {
                return UpdateToSource();
            });
        }
        /// <summary>
        /// 抓取Pair组
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<IPair> FetchPairs(Predicate<APMCore.Model.Pair> predicate) {
            foreach (APMCore.Model.Pair source in FetchPairsHelper(predicate)) {
                yield return new Pair(source);
            }
        }
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="source">数据源</param>
        public async Task CopyPropertiesAsync(IContainer source) {
            await Task.Run(() => {
                CopyProperties(source as ContainerBase);
                Filter = source.Filter;
            });
        }


        /// <summary>
        /// 添加Pair
        /// </summary>
        /// <returns>添加的Pair</returns>
        public async Task<IPair> AddPairAsync() {
            Pair pair = await Task.Run(() => {
                APMCore.Model.Pair source = PairBase.Create(ContainerUID);
                return new Pair(source) {
                    DataBase = DataBase,
                    UpdateMethod = UpdateMethod.Insert
                };
            });
            await Task.Run(() => {
                lock (_pairsLocker) {
                    AddPairHelper(pair);
                }
            });
            return pair;
        }
        /// <summary>
        /// 移除Pair
        /// </summary>
        /// <param name="pair">要移除的Pair</param>
        public async Task<bool> RemovePairAsync(IPair pair) {
            var p = pair as Pair;
            p.UpdateMethod = UpdateMethod.Delete;
            return await Task.Run(() => {
                lock (_pairsLocker) {
                    return RemovePairHelper(p);
                }
            });
        }
        /// <summary>
        /// 移除空Container
        /// </summary>
        /// <returns>移除的Cotnainer数量</returns>
        public async Task<int> ClearEmptyPairsAsync() {
            int removeCount = 0;

            await Task.Run(() => {
                for (int i = 0; i < Pairs.Count; i++) {
                    var p = Pairs[i] as Pair;
                    if (p.IsEmpty) {
                        RemovePairHelper(p);
                        --i;
                        ++removeCount;
                    }
                }
            });

            return removeCount;
        }
        /// <summary>
        /// 设置Container头像
        /// </summary>
        /// <param name="filePath">头像文件路径</param>
        public async Task<bool> SetAvatarAsync(string filePath) {
            return await Task.Run(() => {
                string fileExt = Path.GetExtension(filePath);
                string avatarName = $"C{ContainerUID}{fileExt}";
                try {
                    File.Copy(filePath, $@"{DataAvatarsFolderName}\{avatarName}", true);
                }
                catch {
                    return false;
                }
                Avatar = avatarName;
                return true;
            });
        }
        #endregion

        #region 私有辅助方法
        private void AddPairHelper(Pair pair) {
            pair.UpdateMethod = UpdateMethod.Insert;
            LoadPairHelper(pair);
        }
        private bool RemovePairHelper(Pair pair) {
            pair.UpdateMethod = UpdateMethod.Delete;
            return _pairs.Remove(pair);
        }
        private void LoadPairHelper(Pair pair) {
            _pairs.Add(pair);
            _workPairs.Add(pair);
        }
        private void ClearPairsHelper() {
            _workPairs.Clear();
            _pairs.Clear();
        }
        private void ReloadPairs() {
            ClearPairsHelper();
            foreach (APMCore.Model.Pair source in FetchPairsHelper((p) => true)) {
                Pair pair = new Pair(source) {
                    DataBase = DataBase,
                    UpdateMethod = UpdateMethod.Update
                };
                LoadPairHelper(pair);
            }
        }
        private void UnloadPairs() {
            ClearPairsHelper();
        }
        #endregion

        #region 特殊方法
        public override string ToString() {
            return $"[{Header}]: {Description}";
        }
        #endregion
        #endregion
    }
}
