using System.Collections.Generic;

namespace APMCore {
    /// <summary>
    /// 全局常量与静态只读字段
    /// </summary>
    public static class APM {
        /// <summary>
        /// 表
        /// </summary>
        public static readonly string[] Tables = new string[] { FiltersTable, ContainersTable, PairsTable };

        /// <summary>
        /// 表的主键
        /// </summary>
        public static readonly Dictionary<string, string> TableUID = new Dictionary<string, string>() {
            [FiltersTable] = FilterUID,
            [ContainersTable] = ContainerUID,
            [PairsTable] = PairUID
        };

        /// <summary>
        /// Filters表名
        /// </summary>
        public const string FiltersTable = "Filters";
        /// <summary>
        /// Filters表字段名
        /// </summary>
        public const string FilterUID = "FilterUID"; //Filter的UID
        public const string FilterName = "Name"; //Filter的名称
        public const string FilterIdentifier = "Identifier"; //Filter的特征标
        public const string FilterIsOn = "IsOn"; //Filter的开关状态

        /// <summary>
        /// Containers表名
        /// </summary>
        public const string ContainersTable = "Containers";
        /// <summary>
        /// Containers表字段名
        /// </summary>
        public const string ContainerUID = "ContainerUID"; //Container的UID
        public const string ContainerFilter = "FilterUID"; //Container所属过滤器
        public const string ContainerHeader = "Header"; //Container的标头
        public const string ContainerDescrption = "Description"; //Container的简述
        public const string ContainerAvatar = "Avatar"; //Container的头像路径

        /// <summary>
        /// Pairs表名
        /// </summary>
        public const string PairsTable = "Pairs";
        /// <summary>
        /// Pairs表字段名
        /// </summary>
        public const string PairUID = "PairUID"; //记录组的UID
        public const string PairContainer = "ContainerUID"; //记录组所属容器
        public const string PairTitle = "Title"; //记录组标题
        public const string PairDetail = "Detail"; //记录组细节

        /// <summary>
        /// 创建表的语句组
        /// </summary>
        public static readonly string[] TableCreaters = new string[] {
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
    }
}
