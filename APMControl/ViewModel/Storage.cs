using APMControl.APMException;
using APMControl.Interface;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APMControl {
    using ContainerCollection = DispatchedObservableCollection<Container>;
    using FilterCollection = DispatchedObservableCollection<Filter>;

    public sealed class Storage : APMCore.ViewModel.StorageBase, IStorage {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 新建Container所用的模板
        /// </summary>
        public ContainerTemplate ContainerTemplate {
            get {
                return _containerTemplate;
            }
        }
        /// <summary>
        /// 可视化的Filter列表
        /// </summary>
        public FilterCollection Filters {
            get {
                return _filters;
            }
        }
        /// <summary>
        /// 可视化的Container列表
        /// </summary>
        public ContainerCollection Containers {
            get {
                return _containers;
            }
        }
        #endregion

        #region 私有字段
        private readonly object _filtersLocker;
        private readonly object _containersLocker;
        #endregion

        #region 后备字段
        private readonly ContainerTemplate _containerTemplate;
        private SQLiteTransaction _transaction;
        private readonly FilterCollection _filters;
        private readonly ContainerCollection _containers;
        #endregion
        #endregion

        #region 构造方法
        /// <summary>
        /// 创建一个储存库
        /// </summary>
        /// <param name="conn">连接的数据库</param>
        public Storage(SQLiteConnection conn) : base(conn) {
            _transaction = conn.BeginTransaction();

            _filtersLocker = new object();
            _containersLocker = new object();

            _filters = new FilterCollection();
            _containers = new ContainerCollection();
            _containerTemplate = new ContainerTemplate(conn);
        }
        #endregion

        #region 方法
        #region 公共方法
        /// <summary>
        /// 更新数据至源
        /// </summary>
        public async Task UpdateToSourceAsync() {
            try {
                await _transaction.CommitAsync();
            }
            catch (Exception) {
                throw new StorageFileIOException();
            }
            finally {
                _transaction = DataBase.BeginTransaction();
            }
        }
        /// <summary>
        /// 重新加载Filters与Containers
        /// </summary>
        public async Task ReloadAsync() {
            await ReloadFiltersAsync();
            await ReloadContainersAsync();
        }
        /// <summary>
        /// 重新加载Filters
        /// </summary>
        public async Task ReloadFiltersAsync() {
            await Task.Run(() => {
                lock (_filtersLocker) {
                    _filters.Clear();
                    foreach (APMCore.Model.Filter filterSource in base.FetchFiltersSource((f) => true)) {
                        Filter filter = new Filter(filterSource) {
                            DataBase = DataBase,
                            UpdateMethod = APMCore.UpdateMethod.Update
                        };
                        LoadFilterHelper(filter);
                    }
                }
            });
        }
        /// <summary>
        /// 重新加载Containers
        /// </summary>
        public async Task ReloadContainersAsync() {
            await Task.Run(() => {
                lock (_containersLocker) {
                    _containers.Clear();
                    foreach (Filter filter in Filters) {
                        if (!filter.IsOn) {
                            continue;
                        }
                        LoadContainersHelper(filter.FetchContainers((c) => true));
                    }
                }
            });
        }
        /// <summary>
        /// 按key搜索Containers
        /// </summary>
        /// <param name="key"></param>
        public async Task ReloadContainersByKeywordAsync(string key) {
            await Task.Run(() => {
                lock (_containersLocker) {
                    _containers.Clear();
                    Regex re = new Regex($@"{key}");
                    Predicate<APMCore.Model.Container> predicate = (APMCore.Model.Container c) => {
                        if (re.IsMatch(c.Header) || re.IsMatch(c.Description)) {
                            return true;
                        }
                        return false;
                    };
                    foreach (Filter filter in _filters) {
                        LoadContainersHelper(filter.FetchContainers(predicate));
                    }
                }
            });
        }
        /// <summary>
        /// 反转所有Filters开关状态
        /// </summary>
        public async Task InverseFiltersAsync() {
            foreach (Filter filter in _filters) {
                filter.IsOn = !filter.IsOn;
            }
            await ReloadContainersAsync();
        }
        /// <summary>
        /// 打开所有Filters
        /// </summary>
        public async Task ToggleOnAllFiltersAsync() {
            foreach (Filter filter in _filters) {
                filter.IsOn = true;
            }
            await ReloadContainersAsync();
        }
        /// <summary>
        /// 聚焦Containers
        /// </summary>
        public async Task FocusContainersAsync(Filter filter) {
            await Task.Run(() => {
                lock (_containersLocker) {
                    if (filter == null || filter.IsOn == false) {
                        foreach (Container container in _containers) {
                            container.Opacity = 1;
                        }
                    } else {
                        foreach (Container container in _containers) {
                            if (container.FilterUID == filter.FilterUID) {
                                container.Opacity = 1;
                            } else {
                                container.Opacity = 0.25;
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 添加一个Filter
        /// </summary>
        /// <returns></returns>
        public async Task<Filter> AddFilterAsync() {
            Filter filter = await Task.Run(() => {
                APMCore.Model.Filter source = APMCore.ViewModel.FilterBase.Create(_filterUIDGenerator.Get());
                Filter filter = new Filter(source) {
                    DataBase = DataBase
                };
                return filter;
            });
            lock (_filtersLocker) {
                filter.UpdateToSource(APMCore.UpdateMethod.Insert);
                filter.UpdateMethod = APMCore.UpdateMethod.Update;
                LoadFilterHelper(filter);
            };
            return filter;
        }
        /// <summary>
        /// 移除一个Filter
        /// </summary>
        /// <param name="filter"></param>
        public async Task<bool> RemoveFilterAsync(Filter filter) {
            return await Task.Run(() => {
                filter.UpdateMethod = APMCore.UpdateMethod.Delete;
                if (ContainerTemplate.Filter == filter) {
                    ContainerTemplate.Filter = null;
                }
                lock (_filtersLocker) {
                    return RemoveFilterHelper(filter);
                }
            });
        }
        /// <summary>
        /// 移除所有空Filter
        /// </summary>
        /// <returns>移除的Filter数量</returns>
        public async Task<int> ClearEmptyFiltersAsync() {
            return await Task.Run(() => {
                int removeCount = 0;
                lock (_filtersLocker) {
                    for (int i = 0; i < Filters.Count; i++) {
                        if (Filters[i].IsEmpty) {
                            RemoveFilterHelper(Filters[i]);
                            --i;
                            removeCount += 1;
                        }
                    }
                }
                return removeCount;
            });
        }

        /// <summary>
        /// 添加一个Container
        /// </summary>
        /// <returns></returns>
        public async Task<Container> AddContainerAsync() {
            await Task.Run(() => {
                if (_containerTemplate.Filter == null) {
                    foreach (Filter filter in Filters) {
                        if (filter.IsOn) {
                            _containerTemplate.Filter = filter;
                            break;
                        }
                    }
                    if (_containerTemplate.Filter == null) {
                        if (Filters.Count > 0) {
                            _containerTemplate.Filter = Filters[0];
                        }
                    }
                }

            });
            if (ContainerTemplate.Filter == null) {
                return null;
            }
            Container container = await _containerTemplate.MakeInstanceAsync(_containerUIDGenerator.Get());
            lock (_containersLocker) {
                LoadContainerHelper(container);
            }
            return container;
        }
        /// <summary>
        /// 移除一个Container
        /// </summary>
        /// <param name="container"></param>
        public async Task<bool> RemoveContainerAsync(Container container) {
            return await Task.Run(() => {
                lock (_containersLocker) {
                    container.UpdateMethod = APMCore.UpdateMethod.Delete;
                    return RemoveContainerHelper(container);
                }
            });
        }
        /// <summary>
        /// 移除所有空Container
        /// </summary>
        /// <returns>移除的Container数量</returns>
        public async Task<int> ClearEmptyContainersAsync() {
            return await Task.Run(() => {
                int removeCount = 0;
                lock (_containersLocker) {
                    for (int i = 0; i < Containers.Count; i++) {
                        if (Containers[i].IsEmpty) {
                            RemoveContainerHelper(Containers[i]);
                            --i;
                            removeCount += 1;
                        }
                    }
                }
                return removeCount;
            });
        }
        #endregion

        #region 私有方法
        private bool RemoveFilterHelper(Filter filter) {
            filter.UpdateToSource(APMCore.UpdateMethod.Delete);
            return _filters.Remove(filter);
        }
        private void LoadFilterHelper(Filter filter) {
            filter.UpdateMethod = APMCore.UpdateMethod.Update;
            filter.StatusChanged += Filter_StatusChangedAsync;
            filter.Updated += Filter_Updated;
            _filters.Add(filter);
        }

        private bool RemoveContainerHelper(Container container) {
            container.UpdateToSource(APMCore.UpdateMethod.Delete);
            return _containers.Remove(container);
        }
        private void LoadContainerHelper(Container container) {
            container.UpdateMethod = APMCore.UpdateMethod.Update;
            _containers.Add(container);
        }

        private async void Filter_StatusChangedAsync(object sender, APMCore.FilterStatusSwitchedEventArgs e) {
            Filter filter = sender as Filter;
            filter.UpdateToSource();
            if (filter.IsOn) {
                await Task.Run(() => {
                    LoadContainersHelper(filter.FetchContainers((c) => true));
                });
            } else {
                await Task.Run(() => {
                    UnloadContainersHelper(filter.FetchContainers((c) => true));
                });
            }
        }
        private void Filter_Updated(object sender, APMCore.RecordUpdatedEventArgs e) {
            Filter filter = sender as Filter;
            if (e.UpdateInformation.UpdateMethod == APMCore.UpdateMethod.Delete) {
                UnloadContainersHelper(filter.FetchContainers((c) => true));
            }
        }
        private void LoadContainersHelper(IEnumerable<Container> containers) {
            lock (_containersLocker) {
                foreach (Container container in containers) {
                    LoadContainerHelper(container);
                }
            }
        }
        private void UnloadContainersHelper(IEnumerable<Container> containers) {
            lock (_containersLocker) {
                foreach (Container container in containers) {
                    for (int i = 0; i < Containers.Count; i++) {
                        if (Containers[i].ContainerUID == container.ContainerUID) {
                            _containers.Remove(Containers[i]);
                            --i;
                        }
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}
