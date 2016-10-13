/**
 * Created by VolkovA on 27.02.14.
 */

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace selenium.core.Framework.Browser {
    public class FirefoxDriverManager : DriverManager {
        private FirefoxDriver _driver;
        public BrowserSettings Settings { get; }

        public FirefoxDriverManager(BrowserSettings browserSettings) {
            Settings = browserSettings;
        }

        #region DriverManager Members

        public IWebDriver GetDriver() {
            return _driver;
        }

        public void InitDriver() {
            _driver = new FirefoxDriver();
        }

        public void DestroyDriver() {
            _driver.Quit();
        }

        #endregion
    }
}