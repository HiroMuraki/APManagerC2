namespace APMCore {
    /// <summary>
    /// 全局常量与静态只读字段
    /// </summary>
    public static class APM {
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
    }
}
