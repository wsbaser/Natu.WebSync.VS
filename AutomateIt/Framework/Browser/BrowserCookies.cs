namespace selenium.core.Framework.Browser {
    public class BrowserCookies:DriverFacade {
        public BrowserCookies(Browser browser) : base(browser) {
        }

        /// <summary>
        /// �������� ��� Cookie
        /// </summary>
        public void Clear() {
            Driver.Manage().Cookies.DeleteAllCookies();
        }
    }
}