using APMCore.ViewModel.Helper;
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
        /// 创建一个关联到指定数据库连接的Storage
        /// </summary>
        /// <param name="conn">关联的数据库</param>
        public StorageBase(SQLiteConnection conn) {
            _database = conn;
            SQLiteCommand cmd = new SQLiteCommand(conn);
            _filterUIDGenerator = new UIDGenerator(GetInitUID(cmd, APM.FilterUID, APM.FiltersTable));
            _containerUIDGenerator = new UIDGenerator(GetInitUID(cmd, APM.ContainerUID, APM.ContainersTable));
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
                    Model.Filter filter = FilterHelper.FetchFrom(results);
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
                        Model.Container container = ContainerHelper.FetchFrom(results);
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
        /// <summary>
        /// 清空储存库
        /// </summary>
        /// <param name="conn">数据库</param>
        public static void EmptyStorage(SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            SQLiteTransaction transaction = conn.BeginTransaction();
            cmd.CommandText = $@"Drop Table If Exists {APM.PairsTable}";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $@"Drop Table If Exists {APM.ContainersTable}";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $@"Drop Table If Exists {APM.FiltersTable}";
            cmd.ExecuteNonQuery();
            transaction.Commit();
        }
        /// <summary>
        /// 请空储存库
        /// </summary>
        /// <param name="storageFile">数据库文件</param>
        public static void EmptyStorage(string storageFile) {
            SQLiteConnection conn = new SQLiteConnection($"data source = {storageFile}");
            conn.Open();
            SQLiteTransaction transaction = conn.BeginTransaction();
            EmptyStorage(conn);
            transaction.Commit();
            conn.Close();
        }
        #endregion

        #region 辅助方法
        private long GetInitUID(SQLiteCommand cmd, string uidField, string tableName) {
            cmd.CommandText = $"Select Max({uidField}) From {tableName}";
            object result = cmd.ExecuteScalar();
            return (result is DBNull ? 0 : (long)result) + 1;
        }
        #endregion
        #endregion
    }
}
