namespace APMControl.APMException {
    public class InvalidUserPasswordException : APMException {
        public InvalidUserPasswordException() {

        }
        public InvalidUserPasswordException(string message) : base(message) {

        }
    }
}
