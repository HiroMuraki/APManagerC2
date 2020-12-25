using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using static APMCore.APM;
using System.IO;

namespace APMCore.Helper {
    internal static class StorageHelper {
        /// <summary>
        /// 创建储存表的SQL语句
        /// </summary>
        private static readonly string[] _tableCreater = new string[] {
            //Filters
            $@"Create Table If Not Exists {FiltersTable}
               (
                   [{FilterUID}]        Integer Not Null,
                   [{FilterName}]       Text    Not Null,
                   [{FilterIdentifier}] Text    Not Null,
                   [{FilterIsOn}]       Integer Not Null,
                   Primary Key([{FilterUID}] Autoincrement)
               )",

            //Contaienrs
            $@"Create Table If Not Exists {ContainersTable}
               (
                   [{ContainerUID}]        Integer Not Null,
                   [{ContainerFilter}]     Integer Not Null,
                   [{ContainerHeader}]     Text    Not Null,
                   [{ContainerDescrption}] Text    Not Null,
                   [{ContainerAvatar}]     Text    Not Null,
                   Primary Key([{ContainerUID}] Autoincrement),
                   Foreign Key([{FilterUID}]) References {FiltersTable}([{FilterUID}]) On Update Cascade
               )",

            //Pairs
            $@"Create Table If Not Exists {PairsTable}
               (
                   [{PairUID}]       Integer Not Null,
                   [{PairContainer}] Integer Not Null,
                   [{PairTitle}]     Text    Not Null,
                   [{PairDetail}]    Text    Not Null,
                   Primary Key([{PairUID}] Autoincrement),
                   Foreign Key([{ContainerUID}]) References {ContainersTable}([{ContainerUID}]) On Update Cascade 
               )" 
        };
        /// <summary>
        /// 删除储存表的SQL语句
        /// </summary>
        private static readonly string[] _tableEmptyer = new string[] {
                // Pairs
                $"Drop Table If Exists {PairsTable}",
                // Containers
                $"Drop Table If Exists {ContainersTable}",
                // Filters
                $"Drop Table If Exists {FiltersTable}",
            };

        /// <summary>
        /// 建立空表
        /// </summary>
        /// <param name="conn"></param>
        public static void Create(SQLiteConnection conn) {
            ExecuteSqlCore(conn, _tableCreater);
        }
        public static void Create(string filePath) {
            if (!File.Exists(filePath)) {
                SQLiteConnection.CreateFile(filePath);
            }
            ExecuteSqlCore(filePath, _tableCreater);
        }
        /// <summary>
        /// 清空表内容
        /// </summary>
        /// <param name="conn"></param>
        public static void Empty(SQLiteConnection conn) {
            ExecuteSqlCore(conn, _tableEmptyer);
        }
        public static void Empty(string filePath) {
            ExecuteSqlCore(filePath, _tableEmptyer);
        }

        private static void ExecuteSqlCore(SQLiteConnection conn, string[] sqls) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            foreach (var sql in sqls) {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }
        private static void ExecuteSqlCore(string filePath, string[] sqls) {
            SQLiteConnection conn = new SQLiteConnection($"data source = {filePath}");
            conn.Open();
            SQLiteTransaction transaction = conn.BeginTransaction();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            ExecuteSqlCore(conn, sqls);
            transaction.Commit();
        }
    }
}
