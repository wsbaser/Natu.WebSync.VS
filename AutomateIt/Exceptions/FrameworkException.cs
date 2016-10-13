using System;

namespace selenium.core.Exceptions {
    public class FrameworkException : Exception {
        public FrameworkException(string message) : base(message) {
        }
    }
}