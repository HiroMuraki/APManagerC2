using System;
using System.Data.SQLite;

namespace APMCore.ViewModel {
    /// <summary>
    /// Container所储存信息的单元
    /// </summary>
    public class PairBase : APMBase<Model.Pair> {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 关联数据模型唯一标识符
        /// </summary>
        public long PairUID {
            get {
                return _dataSource.PairUID;
            }
        }
        /// <summary>
        /// 所属数据容器的UID
        /// </summary>
        public long ContainerUID {
            get {
                return _dataSource.ContainerUID;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title {
            get {
                return _dataSource.Title;
            }
            set {
                _dataSource.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        /// <summary>
        /// 细节
        /// </summary>
        public string Detail {
            get {
                return _dataSource.Detail;
            }
            set {
                _dataSource.Detail = value;
                OnPropertyChanged(nameof(Detail));
            }
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 数据源
        /// </summary>
        protected readonly new Model.Pair _dataSource;
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        protected PairBase() {

        }
        /// <summary>
        /// 表示一个记录组
        /// </summary>
        /// <param name="source">数据源</param>
        protected PairBase(Model.Pair source) {
            _dataSource = source;
        }
        #endregion

        #region 方法
        #region 保护方法
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="pair">复制源</param>
        protected virtual void CopyProperties(PairBase pair) {
            Title = pair.Title;
            Detail = pair.Detail;
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
        public static UpdateInformation UpdateToSource(Model.Pair source, SQLiteConnection conn, UpdateMethod updateMethod) {
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
        /// 插入一条Pair记录
        /// </summary>
        /// <param name="source">要插入的记录数据模型</param>
        /// <param name="conn">目标数据库</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation Insert(Model.Pair source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Insert Into {APM.PairsTable}({APM.ContainerUID}, {APM.PairTitle}, {APM.PairDetail})
                                 Values({source.ContainerUID}, '{source.Title}', '{source.Detail}')";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Insert, source.PairUID);
        }
        /// <summary>
        /// 更新一条Pair记录
        /// </summary>
        /// <param name="source">要更新的记录数据模型</param>
        /// <param name="conn">目标数据库</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation Update(Model.Pair source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Update {APM.PairsTable} 
                                 Set {APM.PairTitle}  = '{source.Title}', 
                                     {APM.PairDetail} = '{source.Detail}' 
                                 Where {APM.PairUID} == {source.PairUID}";
            int impact = cmd.ExecuteNonQuery();
            return new UpdateInformation(impact, UpdateMethod.Update, source.PairUID);
        }
        /// <summary>
        /// 删除一条Pair记录
        /// </summary>
        /// <param name="source">要删除的记录数据模型</param>
        /// <param name="conn">目标数据库</param>
        /// <returns>更新信息</returns>
        public static UpdateInformation Delete(Model.Pair source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Delete From {APM.PairsTable} 
                                 Where {APM.PairUID} == {source.PairUID}";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Delete, source.PairUID);
        }
        /// <summary>
        /// 新建Pair
        /// </summary>
        /// <param name="uid">唯一标识符</param>
        /// <returns>新建的Filter</returns>
        public static Model.Pair Create(long containerUID) {
            Model.Pair source = new Model.Pair(-1) {
                ContainerUID = containerUID,
                Title = "",
                Detail = ""
            };
            return source;
        }
        /// <summary>
        /// 从指定数据库中抓取数据
        /// </summary>
        /// <param name="conn">指定的数据库</param>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        /// <summary>
        /// 从指定数据库中抓取源数据
        /// </summary>
        /// <param name="conn">指定的数据库</param>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        public static Model.Pair FetchSourceFrom(SQLiteConnection conn, long uid) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select *
                                 From {APM.PairsTable}
                                 Where {APM.PairUID} == {uid}";

            using (SQLiteDataReader result = cmd.ExecuteReader()) {
                result.Read();
                return FetchSourceFrom(result);
            }
        }
        /// <summary>
        /// 从DataReader中抓取源数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Model.Pair FetchSourceFrom(SQLiteDataReader reader) {
            long uid = Convert.ToInt64(reader[APM.PairUID]);
            return new Model.Pair(uid) {
                ContainerUID = Convert.ToInt64(reader[APM.PairContainer]),
                Title = Convert.ToString(reader[APM.PairTitle]),
                Detail = Convert.ToString(reader[APM.PairDetail])
            };
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 添加到数据库
        /// </summary>
        /// <param name="conn">目标数据库</param>
        /// <returns>受影响的记录数</returns>
        protected override UpdateInformation InsertInto(SQLiteConnection conn) {
            return Insert(_dataSource, conn);
        }
        /// <summary>
        /// 从数据库中修改
        /// </summary>
        /// <param name="conn">目标数据库</param>
        /// <returns>受影响的记录数</returns>
        protected override UpdateInformation UpdateTo(SQLiteConnection conn) {
            return Update(_dataSource, conn);
        }
        /// <summary>
        /// 从数据库中删除
        /// </summary>
        /// <param name="conn">指定数据库</param>
        /// <returns>受影响的记录数</returns>
        protected override UpdateInformation DeleteFrom(SQLiteConnection conn) {
            return Delete(_dataSource, conn);
        }
        #endregion
        #endregion
    }
}
