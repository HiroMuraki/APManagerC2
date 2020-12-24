using System.Runtime.Serialization;

namespace APMCore.Model {
    /// <summary>
    /// Container所储存信息的单元的源数据
    /// </summary>
    [DataContract]
    public class Pair {
        /// <summary>
        /// 唯一标识符UID
        /// </summary>
        [DataMember]
        public readonly long PairUID;

        /// <summary>
        /// 所属容器的UID
        /// </summary>
        [DataMember]
        public long ContainerUID;

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title;

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Detail;

        /// <summary>
        /// 构造函数，传入其唯一标识符
        /// </summary>
        /// <param name="pairUID">唯一标识符</param>
        public Pair(long pairUID) {
            PairUID = pairUID;
            ContainerUID = -1;
            Title = "";
            Detail = "";
        }
    }
}
