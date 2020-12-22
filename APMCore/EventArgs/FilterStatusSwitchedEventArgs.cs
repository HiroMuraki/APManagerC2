using System;

namespace APMCore {
    /// <summary>
    /// 过滤器状态切换时所引发的事件
    /// </summary>
    public class FilterStatusSwitchedEventArgs : EventArgs {
        private readonly bool _status;
        public bool Status {
            get {
                return _status;
            }
        }

        public FilterStatusSwitchedEventArgs(bool status) {
            _status = status;
        }
    }
}
