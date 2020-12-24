using APMCore.Model;
using System.Data.SQLite;

namespace APMCore.ViewModel.Helper {
    internal static class ContainerHelper {

        public static Container FetchFrom(SQLiteConnection conn, long containerUID) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select * From {APM.ContainersTable}
                                 Where {APM.ContainerUID} == {containerUID}";
            using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                reader.Read();
                return FetchFrom(cmd.ExecuteReader());
            }
        }

        public static Container FetchFrom(SQLiteDataReader reader) {
            long containerUID = (long)reader[APM.ContainerUID];
            long filterUID = (long)reader[APM.FilterUID];
            string header = reader[APM.ContainerHeader] as string;
            string descrption = reader[APM.ContainerDescrption] as string;
            string avatar = reader[APM.ContainerAvatar] as string;
            Container source = new Container(containerUID) {
                FilterUID = filterUID,
                Header = header,
                Description = descrption,
                Avatar = avatar
            };
            return source;
        }

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

        public static UpdateInformation Delete(Container source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Delete From {APM.ContainersTable} 
                                     Where {APM.ContainerUID} == {source.ContainerUID} ";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Delete, source.ContainerUID);
        }
    }
}
