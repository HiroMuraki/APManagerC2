namespace APMCore {
    /// <summary>
    /// 更新信息
    /// </summary>
    public readonly struct UpdateInformation {
        private readonly int _impacts;
        private readonly UpdateMethod _updateMethod;
        private readonly long _uid;

        /// <summary>
        /// 受影响的记录数
        /// </summary>
        public int Impacts {
            get {
                return _impacts;
            }
        }
        /// <summary>
        /// 更新的操作
        /// </summary>
        public UpdateMethod UpdateMethod {
            get {
                return _updateMethod;
            }
        }
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public long UID {
            get {
                return _uid;
            }
        }

        public UpdateInformation(int impacts, UpdateMethod updateMethod, long uid) {
            _impacts = impacts;
            _updateMethod = updateMethod;
            _uid = uid;
        }
    }
}
