namespace APMCore {
    /// <summary>
    /// UID生成器
    /// </summary>
    public class UIDGenerator {
        private readonly object _locker;
        private long _uid;

        /// <summary>
        /// 返回当前UID
        /// </summary>
        public long CurrentUID {
            get {
                return _uid;
            }
        }

        /// <summary>
        /// 构造一个UID生成器，传入其开始值
        /// </summary>
        /// <param name="initValue">开始值</param>
        public UIDGenerator(long initValue) {
            _uid = initValue;
            _locker = new object();
        }

        /// <summary>
        /// 获取一个UID
        /// </summary>
        /// <returns></returns>
        public long Get() {
            lock (_locker) {
                return _uid++;
            }
        }
    }
}
