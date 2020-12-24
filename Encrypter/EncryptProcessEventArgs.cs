using System;

namespace Encrypter {
    public class EncryptProcessEventArgs : EventArgs {
        private readonly double _process;

        /// <summary>
        /// 加密进度
        /// </summary>
        public double Process {
            get {
                return _process;
            }
        }

        public EncryptProcessEventArgs(double process) {
            _process = process;
        }
    }
}