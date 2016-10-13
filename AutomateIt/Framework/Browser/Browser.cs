/**
 * Created by VolkovA on 26.02.14.
 */

using System;
using OpenQA.Selenium;
using selenium.core.Framework.Service;
using selenium.core.Logging;

namespace selenium.core.Framework.Browser {
    public class Browser {
        public BrowserOptions Options = new BrowserOptions();
        private readonly DriverManager _driverManager;

        public BrowserSettings Settings => _driverManager.Settings;

        public readonly BrowserAction Action;
        public readonly BrowserAlert Alert;
        public readonly BrowserFind Find;
        public readonly BrowserGet Get;
        public readonly BrowserGo Go;
        public readonly BrowserIs Is;
        public readonly BrowserState State;
        public readonly BrowserWait Wait;
        public readonly BrowserJs Js;
        public BrowserWindow Window;
        public BrowserCookies Cookies;

        public Browser(Web web, TestLogger log, DriverManager driverManager) {
            Web = web;
            Log = log;
            _driverManager = driverManager;
            _driverManager.InitDriver();
            Driver = _driverManager.GetDriver();
            Find = new BrowserFind(this);
            Get = new BrowserGet(this);
            Is = new BrowserIs(this);
            Alert = new BrowserAlert(this);
            State = new BrowserState(this);
            Action = new BrowserAction(this);
            Window = new BrowserWindow(this);
            Go = new BrowserGo(this);
            Wait = new BrowserWait(this);
            Js = new BrowserJs(this);
            Cookies = new BrowserCookies(this);
        }

        public TestLogger Log { get; private set; }
        public Web Web { get; private set; }
        public IWebDriver Driver { get; private set; }

        // Уничтожить драйвер(закрывает все открытые окна браузер)
        public void Destroy() {
            _driverManager.DestroyDriver();
        }

        // Пересоздать драйвер
        public void Recreate() {
        }

        public void DisableTimeout() {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        public void EnableTimeout() {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(BrowserTimeouts.FIND));
        }

        public void WithOptions(Action action, bool findSingle = BrowserOptions.FINDSINGLE_DEFAULT) {
            var memento = (BrowserOptions) Options.Clone();
            Options.FindSingle = findSingle;
            action.Invoke();
            Options = memento;
        }
    }

    public class BrowserOptions : ICloneable {
        /// <summary>
        /// Если при поиске элемента по селектору найдено несколько кидать исключение
        /// </summary>
        public const bool FINDSINGLE_DEFAULT = true;

        /// <summary>
        /// Ожидать завершения Ajax запросов перед выполнением клика
        /// </summary>
        public const bool WAIT_WHILE_AJAX_BEFORE_CLICK = true;

        public bool FindSingle = FINDSINGLE_DEFAULT;
        public bool WaitWhileAjaxBeforeClick = WAIT_WHILE_AJAX_BEFORE_CLICK;

        public object Clone() {
            var options = new BrowserOptions {
                                                 FindSingle = FindSingle,
                                                 WaitWhileAjaxBeforeClick = WaitWhileAjaxBeforeClick
                                             };
            return options;
        }
    }

    public static class BrowserTimeouts {
        /// <summary>
        /// Иногда после выполнения некоторого действия страница подвисает и просто ничего не происходит.
        /// Данное значение определяет максимальное время ожидания пока отрабатывает Java Script
        /// </summary>
        public const int JS = 10;
        /// <summary>
        /// Таймаут ожидания при поиске элемента по умолчанию
        /// </summary>
        public const int FIND = 10;
        /// <summary>
        /// Таймаут ожидания пока отображается прогресс
        /// </summary>
        public const int AJAX = 30;
        /// <summary>
        /// Таймаут ожидания пока подгружаются компоненты страницы
        /// </summary>
        public const int PAGE_LOAD = 30;
        /// <summary>
        /// Таймаут ожидания пока загрузится файл
        /// </summary>
        public const int FILE_DOWNLOAD = 30;
    }
}