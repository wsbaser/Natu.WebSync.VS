namespace selenium.core.Exceptions {
    public static class Throw {
        public static FrameworkException FrameworkException(string msg,params object[] args) {
            return new FrameworkException(string.Format(msg, args));
        }

        public static TestException TestException(string msg, params object[] args) {
            return new TestException(string.Format(msg, args));
        }
    }
}