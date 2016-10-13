/**
* Created by VolkovA on 27.02.14.
*/

using System;
using OpenQA.Selenium;
using selenium.core.Framework.Service;
using selenium.core.Logging;

namespace selenium.core.Framework.Browser {
    public abstract class DriverFacade {
        public DriverFacade(Browser browser) {
            Browser = browser;
        }

        protected Browser Browser { get; private set; }

        protected Web Web {
            get { return Browser.Web; }
        }

        protected TestLogger Log {
            get { return Browser.Log; }
        }

        protected IWebDriver Driver {
            get { return Browser.Driver; }
        }

        /// <summary>
        /// Повторять действие если возникло StaleReferenceException
        /// </summary>
        /// <param name="func">Действие</param>
        public T RepeatAfterStale<T>(Func<T> func) {
            const int TRY_COUNT = 3;
            T result = default(T);
            for (int i = 0; i < TRY_COUNT; i++) {
                try {
                    result = func.Invoke();
                    break;
                }
                catch (StaleElementReferenceException e) {
                    Log.Exception(e);
                    if (i == TRY_COUNT - 1)
                        throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Повторять действие если возникло StaleReferenceException
        /// </summary>
        /// <param name="action">действие</param>
        public void RepeatAfterStale(Action action) {
            RepeatAfterStale(
                () => {
                    action.Invoke();
                    return true;
                });
        }
    }
}