using System.Runtime.Serialization;

namespace APMCore.Model {
    /// <summary>
    /// 用于储存Pair记录组的源数据
    /// </summary>
    [DataContract]
    public class Container {
        /// <summary>
        /// 唯一标识符UID
        /// </summary>
        [DataMember]
        public readonly long ContainerUID;

        /// <summary>
        /// 所属过滤器的UID
        /// </summary>
        [DataMember]
        public long FilterUID;

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Header;

        /// <summary>
        /// 简要描述
        /// </summary>
        [DataMember] 
        public string Description;

        /// <summary>
        /// 头像路径
        /// </summary>
        [DataMember]
        public string Avatar;

        /// <summary>
        /// 构造函数，传入其唯一标识符
        /// </summary>
        /// <param name="containerUID">唯一标识符</param>
        public Container(long containerUID) {
            ContainerUID = containerUID;
        }
    }
}
