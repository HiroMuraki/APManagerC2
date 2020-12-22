using System;

namespace APMControl.APMException {
    public class APMException : Exception {
        public APMException() {

        }
        public APMException(string message) : base(message) {

        }
    }
}
