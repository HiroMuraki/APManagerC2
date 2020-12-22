using System.Runtime.Serialization;

namespace APMCore.Model {
    /// <summary>
    /// 用户配置信息的源数据
    /// </summary>
    [DataContract]
    public class UserData {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string UserName;

        /// <summary>
        /// 用户密码
        /// </summary>
        [DataMember]
        public string UserPassword;

        /// <summary>
        /// 附加描述
        /// </summary>
        [DataMember]
        public string Description;

        /// <summary>
        /// 头像路径
        /// </summary>
        [DataMember]
        public string Avatar;

        /// <summary>
        /// 储存库路径
        /// </summary>
        [DataMember]
        public string Storage;

        /// <summary>
        /// 是否为编辑模式
        /// </summary>
        [DataMember]
        public bool IsEditMode;

        /// <summary>
        /// 指示容器列数
        /// </summary>
        [DataMember]
        public int ColumnSize;
    }
}
