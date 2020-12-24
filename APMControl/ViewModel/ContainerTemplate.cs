using APMControl.Interface;
using APMCore;
using APMCore.ViewModel;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace APMControl {
    using PairCollection = DispatchedObservableCollection<IPair>;
    public sealed class ContainerTemplate : ContainerBase, IContainer {
        #region 属性
        #region 公共属性
        public PairCollection Pairs {
            get {
                return _pairs;
            }
        }
        public IFilter Filter {
            get {
                return _filter;
            }
            set {
                _filter = value;
                if (value != null) {
                    _dataSource.FilterUID = (value as Filter).FilterUID;
                }
                OnPropertyChanged(nameof(Filter));
            }
        }
        #endregion

        #region 私有字段
        private readonly object _pairsLocker;
        #endregion

        #region 后备字段
        private IFilter _filter;
        private readonly PairCollection _pairs;
        #endregion
        #endregion

        #region 构造函数
        public ContainerTemplate(SQLiteConnection conn) : base(new APMCore.Model.Container(-1)) {
            DataBase = conn;
            _pairsLocker = new object();
            _pairs = new PairCollection();
            _filter = null;
        }
        #endregion

        #region 方法
        #region 公共方法
        /// <summary>
        /// 打开容器
        /// </summary>
        public async Task OpenAsync() {
            await Task.Run(() => { });
        }
        /// <summary>
        /// 关闭容器
        /// </summary>
        public async Task CloseAsync() {
            await Task.Run(() => { });
        }
        /// <summary>
        /// 保存设置
        /// </summary>
        public async Task<UpdateInformation> UpdateToSourceAsync() {
            return await Task.Run(() => {
                return new UpdateInformation(0, UpdateMethod.Update, -1);
            });
        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="source"></param>
        public async Task CopyPropertiesAsync(IContainer source) {
            Avatar = source.Avatar;
            Header = source.Header;
            Description = source.Description;
            Filter = source.Filter;
            FilterUID = (source as Container).FilterUID;
            await Task.Run(() => {
                lock (_pairsLocker) {
                    _pairs.Clear();
                    foreach (Pair pair in source.Pairs) {
                        _pairs.Add(pair);
                    }
                }
            });
        }
        /// <summary>
        /// 根据Template创建Container实例
        /// </summary>
        /// <param name="containerUID">container的UID</param>
        /// <returns>创建的Container</returns>
        public async Task<Container> MakeInstanceAsync(long containerUID) {
            Container container = await Task.Run(() => {
                APMCore.Model.Container source = new APMCore.Model.Container(containerUID) {
                    Avatar = "",
                    Header = Header,
                    Description = Description,
                    FilterUID = FilterUID
                };
                return new Container(source) {
                    DataBase = DataBase,
                    Filter = Filter,
                };
            });

            if (!string.IsNullOrEmpty(Avatar)) {
                await container.SetAvatarAsync(Avatar);
            }

            container.UpdateToSource(UpdateMethod.Insert);

            foreach (IPair pair in Pairs) {
                IPair sourcePair = await container.AddPairAsync();
                sourcePair.Title = pair.Title;
                sourcePair.Detail = pair.Detail;
            }

            container.UpdateToSource(UpdateMethod.Update);

            return container;
        }

        /// <summary>
        /// 添加Pair
        /// </summary>
        /// <returns>添加的Pair</returns>
        public async Task<IPair> AddPairAsync() {
            Pair pair = await Task.Run(() => {
                APMCore.Model.Pair source = new APMCore.Model.Pair(-1) {
                    ContainerUID = ContainerUID,
                    Title = "",
                    Detail = ""
                };
                return new Pair(source) {
                    DataBase = DataBase,
                    UpdateMethod = UpdateMethod.Insert
                };
            });
            await Task.Run(() => {
                lock (_pairsLocker) {
                    _pairs.Add(pair);
                }
            });
            return pair;
        }
        /// <summary>
        /// 移除Piar
        /// </summary>
        /// <param name="pair">要移除的Pair</param>
        public async Task<bool> RemovePairAsync(IPair pair) {
            return await Task.Run(() => {
                lock (_pairsLocker) {
                    return _pairs.Remove(pair);
                }
            });
        }
        /// <summary>
        /// 清理空Pair
        /// </summary>
        /// <returns></returns>
        public async Task<int> ClearEmptyPairsAsync() {
            int removeCount = 0;

            await Task.Run(() => {
                lock (_pairsLocker) {
                    for (int i = 0; i < Pairs.Count; i++) {
                        var p = Pairs[i] as Pair;
                        if (p.IsEmpty) {
                            _pairs.Remove(p);
                            --i;
                            ++removeCount;
                        }
                    }
                }
            });

            return removeCount;
        }
        /// <summary>
        /// 设置头像
        /// </summary>
        /// <param name="filePath"></param>
        public async Task<bool> SetAvatarAsync(string filePath) {
            return await Task.Run(() => {
                Avatar = filePath;
                return true;
            });
        }
        #endregion
        #endregion
    }
}
