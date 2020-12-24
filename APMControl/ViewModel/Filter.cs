using APMControl.Interface;
using APMCore;
using APMCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace APMControl {
    public sealed class Filter : FilterBase, IFilter {
        #region 事件
        /// <summary>
        /// Filter开关状态改变时引发
        /// </summary>
        public event EventHandler<FilterStatusSwitchedEventArgs> StatusChanged;
        #endregion

        #region 属性
        #region 公共属性
        /// <summary>
        /// 判断Filter是否为空
        /// </summary>
        public bool IsEmpty {
            get {
                return CountContainers() == 0;
            }
        }
        #endregion

        #region 私有字段
        private readonly object _statusLocker;
        #endregion

        #region 构造函数
        public Filter(APMCore.Model.Filter source) : base(source) {
            _statusLocker = new object();
        }
        #endregion
        #endregion

        #region 方法
        #region 公共方法
        /// <summary>
        /// 更新数据至源
        /// </summary>
        /// <returns></returns>
        public override UpdateInformation UpdateToSource(SQLiteConnection conn, UpdateMethod updateMethod) {
            UpdateInformation updateInformation = base.UpdateToSource(conn, updateMethod);
            if (updateInformation.UpdateMethod == UpdateMethod.Delete) {
                foreach (Container container in FetchContainers((c) => true)) {
                    container.UpdateMethod = UpdateMethod.Delete;
                    container.UpdateToSource();
                }
            }
            return updateInformation;
        }
        public async Task<UpdateInformation> UpdateToSourceAsync() {
            return await Task.Run(() => {
                return UpdateToSource();
            });
        }
        public async Task CopyPropertiesAsync(IFilter filter) {
            await Task.Run(() => {
                CopyProperties(filter as Filter);
            });
        }

        /// <summary>
        /// 切换开关状态，并引发StatusChanged事件
        /// </summary>
        public async Task ToggleAsync() {
            await Task.Run(() => {
                lock (_statusLocker) {
                    Toggle();
                    StatusChanged?.Invoke(this, new FilterStatusSwitchedEventArgs(IsOn));
                }
            });
        }

        /// <summary>
        /// 创建一个Container，其Filter为该实例
        /// </summary>
        /// <param name="containerUID">Container的唯一标识符</param>
        /// <returns>创建的Container</returns>
        public async Task<Container> CreateContainerAsync(long containerUID) {
            Container container = await Task.Run(() => {
                APMCore.Model.Container source = ContainerBase.Create(containerUID, FilterUID);
                return new Container(source) {
                    Filter = this,
                    DataBase = DataBase
                };
            });
            return container;
        }
        /// <summary>
        /// 抓取Container
        /// </summary>
        /// <param name="predicate">谓词</param>
        /// <returns>抓取的Container迭代器</returns>
        public IEnumerable<Container> FetchContainers(Predicate<APMCore.Model.Container> predicate) {
            foreach (APMCore.Model.Container source in FetchContainersHelper(predicate)) {
                Container container = new Container(source) {
                    Filter = this,
                    DataBase = DataBase
                };
                yield return container;
            }
        }
        #endregion

        #region 特殊方法
        public override string ToString() {
            return Name;
        }
        #endregion
        #endregion
    }
}
