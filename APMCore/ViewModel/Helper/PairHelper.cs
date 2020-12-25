using APMCore.Model;
using System.Data.SQLite;

namespace APMCore.ViewModel.Helper {
    internal static class PairHelper {
        /// <summary>
        /// 从指定数据库中抓取源
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="pairUID"></param>
        /// <returns></returns>
        public static Pair FetchFrom(SQLiteConnection conn, long pairUID) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select * From {APM.PairTitle}
                                 Where {APM.PairUID} == {pairUID}";
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
        public static Pair FetchFrom(SQLiteDataReader reader) {
            long pairUID = (long)reader[APM.PairUID];
            long containerUID = (long)reader[APM.PairContainer];
            string title = (string)reader[APM.PairTitle];
            string detail = (string)reader[APM.PairDetail];
            Pair source = new Pair(pairUID) {
                ContainerUID = containerUID,
                Title = title,
                Detail = detail
            };
            return source;
        }
        /// <summary>
        /// 向指定数据库中更新记录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static UpdateInformation Update(Pair source, SQLiteConnection conn) {
            string sql = $@"Update {APM.PairsTable} 
                            Set {APM.PairTitle}  = '{source.Title}', 
                                {APM.PairDetail} = '{source.Detail}' 
                            Where {APM.PairUID} == {source.PairUID}";
            return ExecuteSqlCore(source, conn, sql, UpdateMethod.Update);
        }
        /// <summary>
        /// 向指定数据库中插入一条记录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static UpdateInformation Insert(Pair source, SQLiteConnection conn) {
            string sql = $@"Insert Into {APM.PairsTable}
                                       ({APM.ContainerUID}, 
                                        {APM.PairTitle},
                                        {APM.PairDetail})
                                 Values({source.ContainerUID},
                                       '{source.Title}',
                                       '{source.Detail}')";
            return ExecuteSqlCore(source, conn, sql, UpdateMethod.Insert);
        }
        /// <summary>
        /// 从指定数据库中删除记录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static UpdateInformation Delete(Pair source, SQLiteConnection conn) {
            string sql = $@"Delete From {APM.PairsTable} 
                            Where {APM.PairUID} == {source.PairUID}";
            return ExecuteSqlCore(source, conn, sql, UpdateMethod.Delete);
        }

        private static UpdateInformation ExecuteSqlCore(Pair source, SQLiteConnection conn, string sql, UpdateMethod updateMethod) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = sql;
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, updateMethod, source.PairUID);
        }

    }
}
