using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace APMCore.ViewModel {
    /// <summary>
    /// 用于储存Pair记录组
    /// </summary>
    public class ContainerBase : APMBase<Model.Container> {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 关联数据模型的唯一标识符，为只读属性
        /// </summary>
        public long ContainerUID {
            get {
                return _dataSource.ContainerUID;
            }
        }
        /// <summary>
        /// 所属过滤器唯一标识符
        /// </summary>
        public long FilterUID {
            get {
                return _dataSource.FilterUID;
            }
            set {
                _dataSource.FilterUID = value;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Header {
            get {
                return _dataSource.Header;
            }
            set {
                _dataSource.Header = value;
                OnPropertyChanged(nameof(Header));
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description {
            get {
                return _dataSource.Description;
            }
            set {
                _dataSource.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        /// <summary>
        /// 头像路径
        /// </summary>
        public string Avatar {
            get {
                return _dataSource.Avatar;
            }
            set {
                _dataSource.Avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 关联数据
        /// </summary>
        protected readonly new Model.Container _dataSource;
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        protected ContainerBase() {

        }
        /// <summary>
        /// 表示一个数据容器
        /// </summary>
        /// <param name="source">数据源</param>
        public ContainerBase(Model.Container source) : this() {
            _dataSource = source;
        }
        #endregion

        #region 方法
        #region 保护方法
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="container">复制源</param>
        protected virtual void CopyProperties(ContainerBase container) {
            FilterUID = container.FilterUID;
            Header = container.Header;
            Description = container.Description;
            Avatar = container.Avatar;
        }
        /// <summary>
        /// 获取Pairs
        /// </summary>
        /// <param name="conn">指定的数据库</param>
        protected virtual IEnumerable<Model.Pair> FetchPairsHelper(SQLiteConnection conn, Predicate<Model.Pair> predicate) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select *
                                 From {APM.PairsTable}
                                 Where {APM.PairContainer} == {ContainerUID} ";

            using (SQLiteDataReader results = cmd.ExecuteReader()) {
                while (results.Read()) {
                    Model.Pair pair = PairBase.FetchSourceFrom(results);
                    if (predicate(pair)) {
                        yield return pair;
                    }
                }
            }
        }
        protected virtual IEnumerable<Model.Pair> FetchPairsHelper(Predicate<Model.Pair> predicate) {
            return FetchPairsHelper(DataBase, predicate);
        }
        /// <summary>
        /// 获取关联的Pair数量
        /// </summary>
        /// <returns></returns>
        protected virtual long CountPairs() {
            return CountPairs(DataBase);
        }
        protected virtual long CountPairs(SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select Count(*)
                                 From {APM.PairsTable}
                                 Where {APM.PairContainer} == {ContainerUID}";
            return (long)cmd.ExecuteScalar();
        }
        #endregion

        #region 公共静态方法
        /// <summary>
        /// 更新数据至数据库
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="conn">目标数据库</param>
        /// <param name="updateMethod">更新操作</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation UpdateToSource(Model.Container source, SQLiteConnection conn, UpdateMethod updateMethod) {
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
        /// 插入一条Contaienr记录
        /// </summary>
        /// <param name="source">要插入的记录数据模型</param>
        /// <param name="conn">目标数据库</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation Insert(Model.Container source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            //插入数据到表
            cmd.CommandText = $@"Insert Into {APM.ContainersTable}({APM.ContainerUID}, 
                                                                   {APM.ContainerFilter}, 
                                                                   {APM.ContainerHeader}, 
                                                                   {APM.ContainerDescrption}, 
                                                                   {APM.ContainerAvatar})
                                 Values({source.ContainerUID}, {source.FilterUID}, '{source.Header}', '{source.Description}', '{source.Avatar}')";
            int impacts = cmd.ExecuteNonQuery();
            //获取插入的UID
            return new UpdateInformation(impacts, UpdateMethod.Insert, source.ContainerUID);
        }
        /// <summary>
        /// 更新一条Container记录
        /// </summary>
        /// <param name="source">要更新的记录数据模型</param>
        /// <param name="conn">目标数据库</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation Update(Model.Container source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Update {APM.ContainersTable} 
                                 Set {APM.ContainerFilter}     = {source.FilterUID}, 
                                     {APM.ContainerHeader}     = '{source.Header}', 
                                     {APM.ContainerDescrption} = '{source.Description}', 
                                     {APM.ContainerAvatar}     = '{source.Avatar}' 
                                 Where {APM.ContainerUID}      == {source.ContainerUID}";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Update, source.ContainerUID);
        }
        /// <summary>
        /// 删除一条Container记录
        /// </summary>
        /// <param name="source">要删除的记录数据模型</param>
        /// <param name="conn">目标数据库</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation Delete(Model.Container source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Delete From {APM.ContainersTable} 
                                 Where {APM.ContainerUID} == {source.ContainerUID} ";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Delete, source.ContainerUID);
        }
        /// <summary>
        /// 新建Container
        /// </summary>
        /// <param name="uid">唯一标识符</param>
        /// <returns>新建的Container</returns>
        public static Model.Container Create(long uid, long filterUID) {
            Model.Container source = new Model.Container(uid) {
                FilterUID = filterUID,
                Header = "",
                Description = "",
                Avatar = ""
            };
            return source;
        }
        /// <summary>
        /// 从数据库中抓取数据源
        /// </summary>
        /// <param name="conn">指定数据库</param>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        public static Model.Container FetchSourceFrom(SQLiteConnection conn, long uid) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select *
                                 From {APM.ContainersTable}
                                 Where {APM.ContainerUID} == {uid} ";

            using (SQLiteDataReader result = cmd.ExecuteReader()) {
                result.Read();
                return FetchSourceFrom(result);
            }
        }
        /// <summary>
        /// 从DataReader中抓取数据源
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Model.Container FetchSourceFrom(SQLiteDataReader reader) {
            long uid = Convert.ToInt64(reader[APM.ContainerUID]);
            return new Model.Container(uid) {
                FilterUID = Convert.ToInt64(reader[APM.ContainerFilter]),
                Header = Convert.ToString(reader[APM.ContainerHeader]),
                Description = Convert.ToString(reader[APM.ContainerDescrption]),
                Avatar = Convert.ToString(reader[APM.ContainerAvatar])
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
        #endregion
        #endregion
    }
}
