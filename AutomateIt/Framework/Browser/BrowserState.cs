/**
* Created by VolkovA on 27.02.14.
*/ // Текущее состояние браузера

using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using selenium.core.Framework.Page;
using selenium.core.Framework.Service;

namespace selenium.core.Framework.Browser {
    public class BrowserState : DriverFacade {
        // Объект для работы со страницей в активном окне браузера
        public IPage Page;

        // Объект для работы с html имитацией алерта, отображаемой в активной странице браузера
        public IAlert HtmlAlert;

        // Объект для работы с системным алертом, отображаемым в активной странице браузера
        public IAlert SystemAlert;

        /// <summary>
        /// Идентификатор текущего окна
        /// </summary>
        public string CurrentWindowHandle;

        public BrowserState(Browser browser)
            : base(browser) {
        }

        public IAlert GetActiveAlert() {
            return SystemAlert ?? HtmlAlert;
        }

        /// <summary>
        /// Приведение текущего html алерта к указанному типу
        /// </summary>
        public T HtmlAlertAs<T>() {
            return (T) HtmlAlert;
        }

        /// <summary>
        /// Приведение текущего html алерта к указанному типу
        /// </summary>
        public bool HtmlAlertIs<T>() {
            if (HtmlAlert == null)
                return false;
            return HtmlAlert is T;
        }

        // Приведение текущей страницы к нужному типу
        public T PageAs<T>() {
            return (T) Page;
        }

        // Проверка соответствия класса текущей страницы указанному типу
        public bool PageIs<T>() where T : IPage {
            if (Page == null)
                return false;
            return Page is T;
        }

        // Определение текущего состояния браузера
        public void Actualize() {

            ActualizeSystemAlert();
            if (SystemAlert != null)
                return;
            ActualizeWindow();
            Browser.Wait.WhileAjax(ajaxInevitable: true);
            ActualizePage(new RequestData(Driver.Url,
                                          new List<Cookie>(Driver.Manage().Cookies.AllCookies.AsEnumerable())));
            ActualizeHtmlAlert();
        }

        /// <summary>
        /// Актуализация текущего окна
        /// </summary>
        private void ActualizeWindow() {
            if (Driver.WindowHandles.Last() != CurrentWindowHandle) {
                Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                CurrentWindowHandle = Driver.CurrentWindowHandle;
            }
        }

        /// <summary>
        /// Актуализация Html алерта
        /// </summary>
        public void ActualizeHtmlAlert() {
            HtmlAlert = null;
            if (Page == null)
                return;
            HtmlAlert = Page.Alerts.FirstOrDefault(a => a.IsVisible());
        }

        /// <summary>
        /// Актуализация системного алерта
        /// </summary>
        private void ActualizeSystemAlert() {
            SystemAlert = Browser.Alert.GetSystemAlert();
        }

        /// <summary>
        /// Определение класса для работы с текущей активной страницей браузера
        /// </summary>
        private void ActualizePage(RequestData requestData) {
            Page = null;
            if (requestData.Url.IsFile)
                Page = Web.GetEmailPage(requestData.Url);
            else {
                ServiceMatchResult result = Web.MatchService(requestData);
                if (result != null)
                    Page = result.getService().GetPage(requestData, result.getBaseUrlInfo());
            }
            if (Page != null)
                Page.Activate(Browser, Log);
        }
    }
}