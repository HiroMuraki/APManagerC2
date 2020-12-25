using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using static APMCore.APM;

namespace APMCore.ViewModel.Helper {
    internal static class StorageHelper {
        public static void Create(string filePath) {
            string[] sqls = new string[] {
            //Filters
            $@"Create Table Filters
               (
                   [{FilterUID}]        Integer Not Null,
                   [{FilterName}]       Text    Not Null,
                   [{FilterIdentifier}] Text    Not Null,
                   [{FilterIsOn}]       Integer Not Null,
                   Primary Key([{FilterUID}] Autoincrement)
               )",

            //Contaienrs
            $@"Create Table Containers
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
            $@"Create Table Pairs
               (
                   [{PairUID}]       Integer Not Null,
                   [{PairContainer}] Integer Not Null,
                   [{PairTitle}]     Text    Not Null,
                   [{PairDetail}]    Text    Not Null,
                   Primary Key([{PairUID}] Autoincrement),
                   Foreign Key([{ContainerUID}]) References {ContainersTable}([{ContainerUID}]) On Update Cascade 
               )"
        };
            ExecuteSqlCore(filePath, sqls);
        }

        public static void Empty(string filePath) {
            string[] sqls = new string[] {
                $"Drop Table If Exists {PairsTable}",
                $"Drop Table If Exists {ContainersTable}",
                $"Drop Table If Exists {FiltersTable}",
            };
            ExecuteSqlCore(filePath, sqls);
        }

        private static void ExecuteSqlCore(string filePath, string[] sqls) {
            SQLiteConnection conn = new SQLiteConnection($"data source = {filePath}");
            conn.Open();
            SQLiteTransaction transaction = conn.BeginTransaction();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            foreach (var sql in sqls) {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            transaction.Commit();
        }
    }
}
