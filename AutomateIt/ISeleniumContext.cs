using selenium.core.Framework.Browser;
using selenium.core.Framework.Service;
using selenium.core.Logging;

namespace selenium.core {
    public interface ISeleniumContext {
        Web Web { get; }
        TestLogger Log { get; }
        Browser Browser { get; }
    }
}