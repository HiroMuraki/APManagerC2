using System.Runtime.Serialization;

namespace APMCore.Model {
    /// <summary>
    /// 表示用于筛选Container的过滤器的源数据
    /// </summary>
    [DataContract]
    public class Filter {
        /// <summary>
        /// 唯一标识符UID
        /// </summary>
        [DataMember]
        public readonly long FilterUID;

        /// <summary>
        /// 过滤器名
        /// </summary>
        [DataMember]
        public string Name;

        /// <summary>
        /// 过滤器附加标识，这里为16进制颜色字符串
        /// </summary>
        [DataMember]
        public string Identifier;

        /// <summary>
        /// 过滤器开/关状态
        /// </summary>
        [DataMember]
        public bool IsOn;

        /// <summary>
        /// 构造函数，传入其唯一标识符
        /// </summary>
        /// <param name="filterUID">唯一标识符</param>
        public Filter(long filterUID) {
            FilterUID = filterUID;
            Name = "";
            Identifier = "";
            IsOn = true;
        }
    }
}
