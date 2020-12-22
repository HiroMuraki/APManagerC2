using System;

namespace APMCore {
    /// <summary>
    /// 数据更新时的参数
    /// </summary>
    public class RecordUpdatedEventArgs : EventArgs {
        private readonly UpdateInformation _updateInformation;
        public UpdateInformation UpdateInformation {
            get {
                return _updateInformation;
            }
        }

        public RecordUpdatedEventArgs(UpdateInformation updateInformation) {
            _updateInformation = updateInformation;
        }
    }
}
