using System.ComponentModel;

namespace APMCore {
    /// <summary>
    /// 更新操作
    /// </summary>
    public enum UpdateMethod {
        [Description("插入")]
        Insert,
        [Description("修改")]
        Update,
        [Description("删除")]
        Delete
    }
}
