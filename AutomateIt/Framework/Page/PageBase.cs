/**
 * Created by VolkovA on 27.02.14.
 */

using System.Collections.Generic;
using System.Collections.Specialized;
using OpenQA.Selenium;
using selenium.core.Framework.Browser;
using selenium.core.Framework.Service;
using selenium.core.Logging;

namespace selenium.core.Framework.Page {
    public abstract class PageBase : IPage {
        protected PageBase()
        {
            Data = new Dictionary<string, string>();
            Params = new StringDictionary();
        }

        public Browser.Browser Browser { get; private set; }

        public TestLogger Log { get; private set; }

        public BrowserAction Action {
            get { return Browser.Action; }
        }

        public BrowserAlert Alert {
            get { return Browser.Alert; }
        }

        public BrowserFind Find {
            get { return Browser.Find; }
        }

        public BrowserGet Get {
            get { return Browser.Get; }
        }

        public BrowserGo Go {
            get { return Browser.Go; }
        }

        public BrowserIs Is {
            get { return Browser.Is; }
        }

        public BrowserState State {
            get { return Browser.State; }
        }

        public BrowserWait Wait {
            get { return Browser.Wait; }
        }

        public BrowserJs Js {
            get { return Browser.Js; }
        }

        public BrowserWindow Window {
            get { return Browser.Window; }
        }

        BrowserCookies IPageObject.Cookies {
            get { return Browser.Cookies; }
        }

        #region IPage Members

        /// <summary>
        /// Активизировать страницу
        /// </summary>
        /// <remarks>
        /// Если страница активна, значит через нее можно работать с браузером
        /// </remarks>
        public void Activate(Browser.Browser browser, TestLogger log) {
            Browser = browser;
            Log = log;
            Alerts = new List<IHtmlAlert>();
            ProgressBars = new List<IProgressBar>();
            WebPageBuilder.InitPage(this);
        }

        public List<IProgressBar> ProgressBars { get; private set; }

        public List<IHtmlAlert> Alerts { get; private set; }

        public BaseUrlInfo BaseUrlInfo { get; set; }

        public List<Cookie> Cookies { get; set; }

        public StringDictionary Params { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public void RegisterComponent(IComponent component) {
            if (component is IHtmlAlert)
                Alerts.Add(component as IHtmlAlert);
            else if (component is IProgressBar)
                ProgressBars.Add(component as IProgressBar);
        }

        public T RegisterComponent<T>(string componentName, params object[] args) where T : IComponent {
            var component = CreateComponent<T>(args);
            RegisterComponent(component);
            component.ComponentName = componentName;
            return component;
        }

        public T CreateComponent<T>(params object[] args) where T : IComponent {
            return (T) WebPageBuilder.CreateComponent<T>(this, args);
        }

        #endregion
    }
}