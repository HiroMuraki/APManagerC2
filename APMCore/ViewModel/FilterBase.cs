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
        protected virtual IEnumerable<Model.Container> FetchContainersHelper(SQLiteConnection conn, Predicate<Model.Container> predicate) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select *
                                 From {APM.ContainersTable}
                                 Where {APM.ContainerFilter} == {FilterUID}";

            using (SQLiteDataReader results = cmd.ExecuteReader()) {
                while (results.Read()) {
                    Model.Container source = ContainerBase.FetchSourceFrom(results);
                    if (predicate(source)) {
                        yield return source;
                    }
                }
            }
        }
        protected virtual IEnumerable<Model.Container> FetchContainersHelper(Predicate<Model.Container> predicate) {
            return FetchContainersHelper(DataBase, predicate);
        }
        /// <summary>
        /// 获取关联的Container数量
        /// </summary>
        /// <returns></returns>
        protected virtual long CountContainers() {
            return CountContainers(DataBase);
        }
        protected virtual long CountContainers(SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select Count(*)
                                 From {APM.ContainersTable}
                                 Where {APM.ContainerFilter} == {FilterUID}";
            return (long)cmd.ExecuteScalar();
        }
        #endregion

        #region 静态公共方法
        /// <summary>
        /// 更新数据至数据库
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="conn">目标数据库</param>
        /// <param name="updateMethod">更新操作</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation UpdateToSource(Model.Filter source, SQLiteConnection conn, UpdateMethod updateMethod) {
            switch (updateMethod) {
                case UpdateMethod.Insert:
                    return Insert(source, conn);
                case UpdateMethod.Update:
                    return Update(source, conn);
                case UpdateMethod.Delete:
                    return Delete(source, conn);
                default:
                    throw new InvalidOperationException("无效的更新操作");
            }
        }
        /// <summary>
        /// 插入一条Filter记录
        /// </summary>
        /// <param name="source">要插入的记录数据模型</param>
        /// <param name="conn">目标数据库</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation Insert(Model.Filter source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Insert Into {APM.FiltersTable}({APM.FilterUID}, {APM.FilterName}, {APM.FilterIdentifier}, {APM.FilterIsOn})
                                 Values({source.FilterUID}, '{source.Name}', '{source.Identifier}', {source.IsOn})";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Insert, source.FilterUID);
        }
        /// <summary>
        /// 更新一条Filter记录
        /// </summary>
        /// <param name="source">要更新的记录数据模型</param>
        /// <param name="conn">目标数据库</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation Update(Model.Filter source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Update {APM.FiltersTable} 
                                 Set {APM.FilterName}       = '{source.Name}', 
                                     {APM.FilterIdentifier} = '{source.Identifier}',
                                     {APM.FilterIsOn}       = {source.IsOn}
                                 Where {APM.FilterUID} = {source.FilterUID}";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Update, source.FilterUID);
        }
        /// <summary>
        /// 删除一条Filter记录
        /// </summary>
        /// <param name="source">要删除的记录数据模型</param>
        /// <param name="conn">目标数据库</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation Delete(Model.Filter source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Delete From {APM.FiltersTable}
                                 Where {APM.FilterUID} == {source.FilterUID}";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Delete, source.FilterUID);
        }
        /// <summary>
        /// 新建Filter
        /// </summary>
        /// <param name="uid">唯一标识符</param>
        /// <returns>新建的Filter</returns>
        public static Model.Filter Create(long uid) {
            Model.Filter source = new Model.Filter(uid) {
                Name = "",
                Identifier = "",
                IsOn = true
            };
            return source;
        }
        /// <summary>
        /// 从指定数据库中抓取数据
        /// </summary>
        /// <param name="conn">指定的数据库</param>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        public static Model.Filter FetchSourceFrom(SQLiteConnection conn, long uid) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select *
                                 From {APM.FiltersTable} 
                                 Where {APM.FilterUID} == {uid}";

            using (SQLiteDataReader result = cmd.ExecuteReader()) {
                result.Read();
                return FetchSourceFrom(result);
            }
        }
        /// <summary>
        /// 从DataReader中抓取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Model.Filter FetchSourceFrom(SQLiteDataReader reader) {
            long uid = Convert.ToInt64(reader[APM.FilterUID]);
            return new Model.Filter(uid) {
                Name = Convert.ToString(reader[APM.FilterName]),
                Identifier = Convert.ToString(reader[APM.FilterIdentifier]),
                IsOn = Convert.ToBoolean(reader[APM.FilterIsOn]),
            };
        }
        #endregion

        #region 私有方法
        protected override UpdateInformation InsertInto(SQLiteConnection conn) {
            return Insert(_dataSource, conn);
        }
        protected override UpdateInformation UpdateTo(SQLiteConnection conn) {
            return Update(_dataSource, conn);
        }
        protected override UpdateInformation DeleteFrom(SQLiteConnection conn) {
            return Delete(_dataSource, conn);
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
