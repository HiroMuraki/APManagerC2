using APMCore.Model;
using System;
using System.Data.SQLite;

namespace APMCore.ViewModel.Helper {
    internal static class FilterHelper {

        public static Filter FetchFrom(SQLiteConnection conn, long filterUID) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Select * From {APM.FiltersTable}
                                 Where {APM.FilterUID} == {filterUID}";
            using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                reader.Read();
                return FetchFrom(cmd.ExecuteReader());
            }
        }

        public static Filter FetchFrom(SQLiteDataReader reader) {
            long filterUID = (long)reader[APM.FilterUID];
            string name = reader[APM.FilterName] as string;
            string identifier = reader[APM.FilterIdentifier] as string;
            bool isOn = Convert.ToBoolean(reader[APM.FilterIsOn]);
            Filter source = new Filter(filterUID) {
                Name = name,
                Identifier = identifier,
                IsOn = isOn
            };
            return source;
        }

        public static UpdateInformation Update(Filter source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Insert Into {APM.FiltersTable}
                                       ({APM.FilterUID}, 
                                        {APM.FilterName},
                                        {APM.FilterIdentifier},
                                        {APM.FilterIsOn})
                                 Values({source.FilterUID}, 
                                       '{source.Name}', 
                                       '{source.Identifier}', 
                                        {source.IsOn})";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Delete, source.FilterUID);
        }

        public static UpdateInformation Insert(Filter source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Update {APM.FiltersTable} 
                                 Set {APM.FilterName}       = '{source.Name}', 
                                     {APM.FilterIdentifier} = '{source.Identifier}',
                                     {APM.FilterIsOn}       =  {source.IsOn}
                                 Where {APM.FilterUID} = {source.FilterUID}";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Delete, source.FilterUID);
        }

        public static UpdateInformation Delete(Filter source, SQLiteConnection conn) {
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $@"Delete From {APM.FiltersTable}
                                 Where {APM.FilterUID} == {source.FilterUID}";
            int impacts = cmd.ExecuteNonQuery();
            return new UpdateInformation(impacts, UpdateMethod.Delete, source.FilterUID);
        }
    }
}
