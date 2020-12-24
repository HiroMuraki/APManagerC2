﻿using APMCore.Model;
using System.Data.SQLite;

namespace APMCore.ViewModel.Helper {
    internal static class ContainerHelper {
        /// <summary>
        /// 从指定数据库中抓取源
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="pairUID"></param>
        /// <returns></returns>
        public static Container FetchFrom(SQLiteConnection conn, long containerUID) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select * From {APM.ContainersTable}
                                 Where {APM.ContainerUID} == {containerUID}";
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
        public static Container FetchFrom(SQLiteDataReader reader) {
            long containerUID = (long)reader[APM.ContainerUID];
            long filterUID = (long)reader[APM.FilterUID];
            string header = (string)reader[APM.ContainerHeader];
            string descrption = (string)reader[APM.ContainerDescrption];
            string avatar = (string)reader[APM.ContainerAvatar];
            Container source = new Container(containerUID) {
                FilterUID = filterUID,
                Header = header,
                Description = descrption,
                Avatar = avatar
            };
            return source;
        }
        /// <summary>
        /// 向指定数据库中更新记录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static UpdateInformation Update(Container source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Update {APM.ContainersTable} 
                                 Set {APM.ContainerFilter}     =  {source.FilterUID}, 
                                     {APM.ContainerHeader}     = '{source.Header}', 
                                     {APM.ContainerDescrption} = '{source.Description}', 
                                     {APM.ContainerAvatar}     = '{source.Avatar}' 
                                 Where {APM.ContainerUID}      == {source.ContainerUID}";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Update, source.ContainerUID);
        }
        /// <summary>
        /// 向指定数据库中插入一条记录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static UpdateInformation Insert(Container source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            //插入数据到表
            cmd.CommandText = $@"Insert Into {APM.ContainersTable}
                                           ({APM.ContainerUID}, 
                                            {APM.ContainerFilter}, 
                                            {APM.ContainerHeader}, 
                                            {APM.ContainerDescrption}, 
                                            {APM.ContainerAvatar})
                                     Values({source.ContainerUID}, 
                                            {source.FilterUID}, 
                                           '{source.Header}', 
                                           '{source.Description}',
                                           '{source.Avatar}')";
            int impacts = cmd.ExecuteNonQuery();
            //获取插入的UID
            return new UpdateInformation(impacts, UpdateMethod.Insert, source.ContainerUID);

        }
        /// <summary>
        /// 从指定数据库中删除记录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static UpdateInformation Delete(Container source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Delete From {APM.ContainersTable} 
                                 Where {APM.ContainerUID} == {source.ContainerUID} ";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Delete, source.ContainerUID);
        }
    }
}
