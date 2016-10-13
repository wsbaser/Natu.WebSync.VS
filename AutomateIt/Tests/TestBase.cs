/**
 * Created by VolkovA on 26.02.14.
 */
 
using selenium.core.Framework.Browser;
using selenium.core.Framework.Page;
using selenium.core.Logging;

namespace selenium.core.Tests {
    public abstract class TestBase<P> where P : IPage {
        protected P Page => Browser.State.PageAs<P>();

        protected abstract Browser Browser { get; }

        protected abstract TestLogger Log { get; }

        public BrowserAction Action
        {
            get { return Browser.Action; }
        }

        public BrowserAlert Alert
        {
            get { return Browser.Alert; }
        }

        public BrowserFind Find
        {
            get { return Browser.Find; }
        }

        public BrowserGet Get
        {
            get { return Browser.Get; }
        }

        public BrowserGo Go
        {
            get { return Browser.Go; }
        }

        public BrowserIs Is
        {
            get { return Browser.Is; }
        }

        public BrowserState State
        {
            get { return Browser.State; }
        }

        public BrowserWait Wait
        {
            get { return Browser.Wait; }
        }

        public BrowserJs Js
        {
            get { return Browser.Js; }
        }

        public BrowserWindow Window
        {
            get { return Browser.Window; }
        }

    }
}