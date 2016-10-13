/**
* Created by VolkovA on 27.02.14.
*/ // Методы для работы с окнами/вкладками браузера

namespace selenium.core.Framework.Browser {
    public class BrowserWindow : DriverFacade {
        public BrowserWindow(Browser browser)
            : base(browser) {
        }

        public string Url {
            get { return Driver.Url; }
        }
    }
}