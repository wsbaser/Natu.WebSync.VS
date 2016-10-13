using System;

namespace selenium.core.Exceptions {
    public class TestException:Exception {
        public TestException(string message) : base(message) {
        }
    }
}