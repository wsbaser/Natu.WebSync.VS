/**
* Created by VolkovA on 27.02.14.
*/ // Методы для ожидания изменения состояния браузера

using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using selenium.core.SCSS;

namespace selenium.core.Framework.Browser {
    public class BrowserWait : DriverFacade {
        public BrowserWait(Browser browser)
            : base(browser) {
        }

        public void Until(Func<bool> condition, int timeout = 3, int sleepInterval=100) {
            var wait = new WebDriverWait(new SystemClock(), Driver, TimeSpan.FromSeconds(timeout), TimeSpan.FromMilliseconds(sleepInterval));
            wait.Until(driver => condition.Invoke());
        }

        /// <summary>
        /// Подождать пока элемент отображается на странице
        /// </summary>
        /// <param name="by">Селектор видимого элемента</param>
        /// <param name="timeout">Максимальный период ожидания</param>
        public void WhileElementVisible(string scssSelector, int timeout = BrowserTimeouts.AJAX) {
            WhileElementVisible(Scss.GetBy(scssSelector), timeout);
        }

        public void WhileElementVisible(By by, int timeout = BrowserTimeouts.AJAX) {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(driver => !Browser.Is.Visible(by));
        }

        /// <summary>
        /// Wait until element is not visible
        /// </summary>
        public void ForElementVisible(string scssSelector, int timeout = BrowserTimeouts.FIND) {
            ForElementVisible(Scss.GetBy(scssSelector), null, timeout);
        }

        public void ForElementVisible(By by, By frameBy, int timeout = BrowserTimeouts.FIND) {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(driver => Browser.Is.Visible(by, frameBy));
        }

        /// <summary>
        /// Подождать пока не скроются все зарегистрированные на страницы прогрессы
        /// </summary>
        public void WhilePageInProgress() {
            if(Browser.State.Page==null)
                return;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(BrowserTimeouts.PAGE_LOAD));
            wait.Until(driver => Browser.State.Page.ProgressBars.All(p => !p.IsVisible()));
        }

        public void ForPageProgress() {
            if (Browser.State.Page == null)
                return;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(BrowserTimeouts.JS));
            wait.Until(driver => Browser.State.Page.ProgressBars.All(p => p.IsVisible()));
        }

        /// <summary>
        /// Подождать пока не завершатся Ajax запросы
        /// </summary>
        /// <param name="timeout">максимальное время ожидания пока отработают все ajax запросы</param>
        /// <param name="ajaxInevitable">
        /// true - ajax запрос 100% должен выполниться
        /// если этого не произошло, ожидаем 3 секунды и проверяем еще раз
        /// </param>
        public void WhileAjax(int timeout = BrowserTimeouts.AJAX, bool ajaxInevitable = false) {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            bool waited = false;
            wait.Until(
                driver => {
                    var ajaxActive = Browser.Is.AjaxActive();
                    if (ajaxActive) {
                        waited = true;
                        return false;
                    }
                    return true;
                });
            if (!waited && ajaxInevitable) {
                Browser.Wait.ForAjax(3000);
                wait.Until(driver => !Browser.Is.AjaxActive());
            }
        }

        /// <summary>
        /// Подождать пока не начнется выполнения ajax запросов
        /// </summary>
        public void ForAjax(int miliseconds = 1000) {
            const int POLLING_INTERVAL = 200;
            var count = (int) Math.Ceiling(miliseconds/(decimal) POLLING_INTERVAL);
            for (int i = 0; i < count; i++) {
                if (Browser.Is.AjaxActive())
                    return;
                Thread.Sleep(POLLING_INTERVAL);
            }
        }

        public void ForRedirect(string oldUrl = null, bool waitForAjax=false, bool ajaxInevitable=false) {
            const int POLLING_INTERVAL = 200;
            oldUrl = oldUrl ?? Browser.Window.Url;
            for (int i = 0; i < 10; i++)
            {
                Browser.State.Actualize();
                if (Browser.State.SystemAlert != null || oldUrl != Browser.Window.Url)
                {
                    if (waitForAjax || ajaxInevitable)
                        Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
                    return;
                }
                Thread.Sleep(POLLING_INTERVAL);
            }
            Log.Info("No redirect after click");
        }

        public void ForAlert(int timeout=BrowserTimeouts.AJAX) {
            Browser.Wait.Until(() => {
                Browser.State.ActualizeHtmlAlert();
                return Browser.State.HtmlAlert != null;
            }, timeout);
            Browser.State.Actualize();
        }
    }
}