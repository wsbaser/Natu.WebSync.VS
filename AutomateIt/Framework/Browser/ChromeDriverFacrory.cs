/**
 * Created by VolkovA on 03.03.14.
 */

using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace selenium.core.Framework.Browser {
    public class ChromeDriverFacrory : DriverManager {

        private IWebDriver _driver;

        public ChromeDriverFacrory(BrowserSettings browserSettings) {
            Settings = browserSettings;
        }

        #region DriverManager Members

        public BrowserSettings Settings { get; }

        public void InitDriver() {
            // https://src.chromium.org/viewvc/chrome/trunk/src/chrome/common/pref_names.cc?view=markup

            var options = new ChromeOptions();
            options.AddArgument("--allow-running-insecure-content");
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--test-type");
            options.AddArgument("no-sandbox");
            options.AddUserProfilePreference("download.default_directory", Settings.DownloadDirectory);
            //options.AddUserProfilePreference("download.prompt_for_download", true);
            options.AddUserProfilePreference("safebrowsing.enabled", false);
            options.AddUserProfilePreference("safebrowsing.extended_reporting_enabled", false);
            options.AddUserProfilePreference("safebrowsing.download_feedback_enabled", false);
            options.AddUserProfilePreference("safebrowsing.reporting_enabled", false);
            options.AddUserProfilePreference("safebrowsing.proceed_anyway_disabled", false);

            _driver = new ChromeDriver(options);
        }

        public IWebDriver GetDriver() {
            return _driver;
        }

        public void DestroyDriver() {
            _driver.Quit();
        }

        #endregion
    }
}