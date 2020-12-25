using APMCore.Model;
using System;
using System.Data.SQLite;

namespace APMCore.ViewModel.Helper {
    internal static class FilterHelper {
        /// <summary>
        /// 从指定数据库中抓取源
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="pairUID"></param>
        /// <returns></returns>
        public static Filter FetchFrom(SQLiteConnection conn, long filterUID) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select * From {APM.FiltersTable}
                                 Where {APM.FilterUID} == {filterUID}";
            using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                reader.Read();
                return FetchFrom(cmd.ExecuteReader());
            }
        }
        /// <summary>
        /// 从指定数据库中抓取源
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Filter FetchFrom(SQLiteDataReader reader) {
            long filterUID = (long)reader[APM.FilterUID];
            string name = (string)reader[APM.FilterName];
            string identifier = (string)reader[APM.FilterIdentifier];
            bool isOn = Convert.ToBoolean(reader[APM.FilterIsOn]);
            Filter source = new Filter(filterUID) {
                Name = name,
                Identifier = identifier,
                IsOn = isOn
            };
            return source;
        }
        /// <summary>
        /// 向指定数据库中更新记录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static UpdateInformation Update(Filter source, SQLiteConnection conn) {
            string sql = $@"Update {APM.FiltersTable} 
                            Set {APM.FilterName}       = '{source.Name}', 
                                {APM.FilterIdentifier} = '{source.Identifier}',
                                {APM.FilterIsOn}       =  {source.IsOn}
                            Where {APM.FilterUID} = {source.FilterUID}";
            return ExecuteSqlCore(source, conn, sql, UpdateMethod.Update);
        }
        /// <summary>
        /// 向指定数据库中插入一条记录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static UpdateInformation Insert(Filter source, SQLiteConnection conn) {
            string sql = $@"Insert Into {APM.FiltersTable}
                                       ({APM.FilterUID}, 
                                        {APM.FilterName},
                                        {APM.FilterIdentifier},
                                        {APM.FilterIsOn})
                                 Values({source.FilterUID}, 
                                       '{source.Name}', 
                                       '{source.Identifier}', 
                                        {source.IsOn})";
            return ExecuteSqlCore(source, conn, sql, UpdateMethod.Insert);
        }
        /// <summary>
        /// 从指定数据库中删除记录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static UpdateInformation Delete(Filter source, SQLiteConnection conn) {
            string sql = $@"Delete From {APM.FiltersTable}
                            Where {APM.FilterUID} == {source.FilterUID}";
            return ExecuteSqlCore(source, conn, sql, UpdateMethod.Delete);
        }

        private static UpdateInformation ExecuteSqlCore(Filter source, SQLiteConnection conn, string sql, UpdateMethod updateMethod) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = sql;
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, updateMethod, source.FilterUID);
        }
    }
}
