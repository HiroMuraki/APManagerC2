using System;
using System.Data.SQLite;

namespace APMCore {
    /// <summary>
    /// 用于表示数据可以与数据库交互的接口
    /// </summary>
    internal interface IDataBaseHandler {
        /// <summary>
        /// 连接的数据库
        /// </summary>
        public SQLiteConnection DataBase { get; }
        /// <summary>
        /// 数据更新至数据库时引发的事件
        /// </summary>
        public event EventHandler<RecordUpdatedEventArgs> Updated;
        /// <summary>
        /// 指示UpdateToSource的更新操作
        /// </summary>
        public UpdateMethod UpdateMethod { get; set; }
        /// <summary>
        /// 将数据更新至DataBase连接的数据库
        /// </summary>
        /// <returns>更新信息</returns>
        public UpdateInformation UpdateToSource();
        /// <summary>
        /// 更新至DataBase连接的数据库,指定更新方式
        /// </summary>
        /// <param name="conn">指定数据库</param>
        /// <param name="updateMethod">更新操作</param>
        /// <returns>更新信息</returns>
        public UpdateInformation UpdateToSource(UpdateMethod updateMethod);
        /// <summary>
        /// 依据UpdateMethod的值更新至数据库
        /// </summary>
        /// <param name="conn">指定数据库</param>
        /// <returns>更新信息</returns>
        public UpdateInformation UpdateToSource(SQLiteConnection conn);
        /// <summary>
        /// 更新至数据库,指定更新方式
        /// </summary>
        /// <param name="conn">指定数据库</param>
        /// <param name="updateMethod">更新操作</param>
        /// <returns>更新信息</returns>
        public UpdateInformation UpdateToSource(SQLiteConnection conn, UpdateMethod updateMethod);
    }
}
