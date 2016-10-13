using System;

namespace selenium.core.Framework.Service {
    public class RouterInitializationException : Exception {
        public RouterInitializationException(Exception cause)
            : base("Error in router initialization", cause) {
        }
    }
}