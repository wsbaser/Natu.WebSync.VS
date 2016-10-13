/**
 * Created by VolkovA on 06.03.14.
 */

using selenium.core.Framework.Browser;
using selenium.core.Logging;

namespace selenium.core.Framework.Page {
    public interface IPageObject {
        Browser.Browser Browser { get; }
        TestLogger Log { get; }

        BrowserAction Action { get; }
        BrowserAlert Alert { get; }
        BrowserFind Find { get; }
        BrowserGet Get { get; }
        BrowserGo Go { get; }
        BrowserIs Is { get; }
        BrowserState State { get; }
        BrowserWait Wait { get; }
        BrowserJs Js { get; }
        BrowserWindow Window { get; }
        BrowserCookies Cookies { get; }
    }
}