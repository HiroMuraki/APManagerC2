using System;

namespace APMCore {
    /// <summary>
    /// Container的过滤器被修改所引发的事件
    /// </summary>
    public class FilterChangedEventArgs : EventArgs {
        private long _previousFilterUID;
        private long _currentFilterUID;

        public long PreviousFilterUID {
            get { return _previousFilterUID; }
            set { _previousFilterUID = value; }
        }
        public long CurrentFilterUID {
            get { return _currentFilterUID; }
            set { _currentFilterUID = value; }
        }

        public FilterChangedEventArgs(long previousFilterUID, long currentFilterUID) {
            _previousFilterUID = previousFilterUID;
            _currentFilterUID = currentFilterUID;
        }
    }
}
