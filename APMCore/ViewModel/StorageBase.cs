using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace APMCore.ViewModel {
    /// <summary>
    /// APM的储存库
    /// </summary>
    public abstract class StorageBase : ViewModelBase {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 当前连接的数据库
        /// </summary>
        protected SQLiteConnection DataBase {
            get {
                return _database;
            }
        }
        #endregion

        #region 私有字段
        protected readonly UIDGenerator _filterUIDGenerator;
        protected readonly UIDGenerator _containerUIDGenerator;
        #endregion

        #region 后备字段
        protected readonly SQLiteConnection _database;
        #endregion
        #endregion

        #region 构造方法
        /// <summary>
        /// 默认构造函数，初始化集合
        /// </summary>
        protected StorageBase() {
           
        }
        /// <summary>
        /// 创建一个关联到指定数据库连接的Storage
        /// </summary>
        /// <param name="conn">关联的数据库</param>
        public StorageBase(SQLiteConnection conn) : this() {
            _database = conn;

            SQLiteCommand cmd = new SQLiteCommand(conn);
            foreach (string table in APM.Tables) {
                cmd.CommandText = $"Select Max({APM.TableUID[table]}) From {table}";
                object result = cmd.ExecuteScalar();
                long initUID = (result is DBNull ? 0 : (long)result) + 1;

                switch (table) {
                    case APM.FiltersTable:
                        _filterUIDGenerator = new UIDGenerator(initUID);
                        break;
                    case APM.ContainersTable:
                        _containerUIDGenerator = new UIDGenerator(initUID);
                        break;
                }
            }
        }
        #endregion

        #region 方法
        #region 保护方法
        /// <summary>
        /// 依据条件抓取Filter
        /// </summary>
        /// <param name="predicate">谓词回调</param>
        /// <returns>抓取结果的迭代器</returns>
        protected virtual IEnumerable<Model.Filter> FetchFiltersSource(Predicate<Model.Filter> predicate) {
            SQLiteCommand cmd = new SQLiteCommand(_database);
            cmd.CommandText = $"Select * From {APM.FiltersTable}";

            using (SQLiteDataReader results = cmd.ExecuteReader()) {
                while (results.Read()) {
                    Model.Filter filter = FilterBase.FetchSourceFrom(results);
                    if (predicate(filter)) {
                        yield return filter;
                    }
                }
            }
        }
        /// <summary>
        /// 依据条件抓取Container
        /// </summary>
        /// <param name="predicate">谓词回调</param>
        /// <returns>抓取结果的迭代器</returns>
        protected virtual IEnumerable<Model.Container> FetchContainersSource(Predicate<Model.Container> predicate) {
            SQLiteCommand cmd = new SQLiteCommand(_database);

            foreach (Model.Filter filter in FetchFiltersSource((f) => true)) {
                cmd.CommandText = $@"Select * 
                                     From {APM.ContainersTable}
                                     Where {APM.ContainerFilter} == {filter.FilterUID}";
                using (SQLiteDataReader results = cmd.ExecuteReader()) {
                    while (results.Read()) {
                        Model.Container container = ContainerBase.FetchSourceFrom(results);
                        if (predicate(container)) {
                            yield return container;
                        }
                    }
                }
            }
        }
        #endregion

        #region 公共静态方法
        /// <summary>
        /// 在指定的数据库中创建空储存库
        /// </summary>
        /// <param name="conn">指定的数据库</param>
        public static void CreateEmptyStorage(SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            foreach (string tableCreater in APM.TableCreaters) {
                cmd.CommandText = tableCreater;
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 创建指定的数据储存库文件
        /// </summary>
        /// <param name="conn">文件路径</param>
        public static void CreateEmptyStorage(string storageFile) {
            if (File.Exists(storageFile)) {
                throw new IOException();
            }
            SQLiteConnection.CreateFile(storageFile);
            SQLiteConnection conn = new SQLiteConnection("data source = " + storageFile);
            conn.Open();
            SQLiteTransaction transaction = conn.BeginTransaction();
            CreateEmptyStorage(conn);
            transaction.Commit();
            conn.Close();
        }
        #endregion
        #endregion
    }
}
