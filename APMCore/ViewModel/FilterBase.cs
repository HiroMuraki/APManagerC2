using APMCore.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace APMCore.ViewModel {
    /// <summary>
    /// 表示用于筛选Container的过滤器
    /// </summary>
    public class FilterBase : APMBase<Model.Filter> {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 关联数据模型的唯一标识符
        /// </summary>
        public long FilterUID {
            get {
                return _dataSource.FilterUID;
            }
        }
        /// <summary>
        /// 过滤器名
        /// </summary>
        public string Name {
            get {
                return _dataSource.Name;
            }
            set {
                _dataSource.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        /// <summary>
        /// 过滤器特征标
        /// </summary>
        public string Identifier {
            get {
                return _dataSource.Identifier;
            }
            set {
                _dataSource.Identifier = value;
                OnPropertyChanged(nameof(Identifier));
            }
        }
        /// <summary>
        /// 过滤器开/关状态
        /// </summary>
        public bool IsOn {
            get {
                return _dataSource.IsOn;
            }
            private set {
                _dataSource.IsOn = value;
                OnPropertyChanged(nameof(IsOn));
            }
        }
        #endregion

        #region 私有字段
        /// <summary>
        ///  数据源
        /// </summary>
        private readonly new Model.Filter _dataSource;
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        protected FilterBase() {

        }
        /// <summary>
        /// 表示一个过滤器
        /// </summary>
        /// <param name="source">数据源</param>
        protected FilterBase(Model.Filter source) : this() {
            _dataSource = source;
        }
        #endregion

        #region 方法
        #region 公共方法
        public void Toggle() {
            IsOn = !IsOn;
            UpdateIsOn(IsOn);
        }
        public void ToggleOn() {
            IsOn = true;
            UpdateIsOn(true);
        }
        public void ToggleOff() {
            IsOn = false;
            UpdateIsOn(false);
        }
        #endregion

        #region 保护方法
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="filter">复制源</param>
        protected virtual void CopyProperties(FilterBase filter) {
            Name = filter.Name;
            Identifier = filter.Identifier;
            IsOn = filter.IsOn;
        }
        /// <summary>
        /// 从指定数据库中抓取相关容器
        /// </summary>
        /// <param name="conn">指定的数据库</param>
        /// <returns>迭代返回抓取结果</returns>
        protected virtual IEnumerable<Model.Container> FetchContainersHelper(Predicate<Model.Container> predicate) {
            SQLiteCommand cmd = new SQLiteCommand(DataBase);
            cmd.CommandText = $@"Select *
                                 From {APM.ContainersTable}
                                 Where {APM.ContainerFilter} == {FilterUID}";

            using (SQLiteDataReader results = cmd.ExecuteReader()) {
                while (results.Read()) {
                    Model.Container source = ContainerHelper.FetchFrom(results);
                    if (predicate(source)) {
                        yield return source;
                    }
                }
            }
        }
        /// <summary>
        /// 获取关联的Container数量
        /// </summary>
        /// <returns></returns>
        protected virtual long CountContainers() {
            SQLiteCommand cmd = new SQLiteCommand(DataBase);
            cmd.CommandText = $@"Select Count(*)
                                 From {APM.ContainersTable}
                                 Where {APM.ContainerFilter} == {FilterUID}";
            return (long)cmd.ExecuteScalar();
        }
        #endregion

        #region 私有方法
        protected override UpdateInformation InsertInto() {
            return FilterHelper.Insert(_dataSource, DataBase);
        }
        protected override UpdateInformation UpdateTo() {
            return FilterHelper.Update(_dataSource, DataBase);
        }
        protected override UpdateInformation DeleteFrom() {
            return FilterHelper.Delete(_dataSource, DataBase);
        }
        protected void UpdateIsOn(bool value) {
            SQLiteCommand cmd = new SQLiteCommand(DataBase);
            cmd.CommandText = $@"Update {APM.FiltersTable}
                                 Set {APM.FilterIsOn} = {value}
                                 Where {APM.FilterUID} == {FilterUID}";
            cmd.ExecuteNonQuery();
        }
        #endregion
        #endregion
    }
}
