using System;

namespace APMCore {
    public class StorageUpdatedEventArgs : EventArgs {
        /// <summary>
        /// 更新的Filter数
        /// </summary>
        public int FilterUpdatedCount {
            get {
                return _filtersUpdatedCount;
            }
        }
        /// <summary>
        /// 更新的Container数
        /// </summary>
        public int ContainerUpdatedCount {
            get {
                return _containersUpdatedCount;
            }
        }
        private readonly int _filtersUpdatedCount;
        private readonly int _containersUpdatedCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterUpdatedCount">更新的Filter数</param>
        /// <param name="containerUpdatedCount">更新的Container数</param>
        public StorageUpdatedEventArgs(int filterUpdatedCount,int containerUpdatedCount) {
            _filtersUpdatedCount = filterUpdatedCount;
            _containersUpdatedCount = containerUpdatedCount;
        }
    }
}
