/**
* Created by VolkovA on 27.02.14.
*/ // Методы для проверки состояния страниц

using System.Collections.Generic;
using System.Linq;
using automateit.SCSS;
using OpenQA.Selenium;
using selenium.core.SCSS;

namespace selenium.core.Framework.Browser {
    public class BrowserIs : DriverFacade {
        public BrowserIs(Browser browser)
            : base(browser) {
        }

        /// <summary>
        /// Проверяет что элемент отображается на странице
        /// </summary>
        public bool Visible(string scssSelector) {
            return Visible(ScssBuilder.CreateBy(scssSelector));
        }

        public bool Visible(By by) {
            return Visible(Browser.Driver, by, null);
        }

        public bool Visible(By by, By frameBy) {
            return Visible(Browser.Driver, by, frameBy);
        }

        public bool Visible(ISearchContext context, By by, By frameBy) {
            return RepeatAfterStale(
                () => {
                    IWebElement element = Browser.Find.ElementFastS(context, by, frameBy,false);
                    return element != null && element.Displayed;
                });
        }

        /// <summary>
        /// Проверяет имеется ли у элемента указанный класс
        /// </summary>
        public bool HasClass(string scssSelector, string className) {
            return HasClass(ScssBuilder.CreateBy(scssSelector), null, className);
        }

        public bool HasClass(By by, By frameBy, string className) {
            IWebElement element = Browser.Find.ElementFastS(by,frameBy);
            if (element == null)
                return false;
            return HasClass(element, className);
        }

        public bool HasClass(IWebElement element, string className) {
            return element.GetAttribute("class").Split(' ').Select(c => c.Trim()).Contains(className);
        }

        /// <summary>
        /// Существует ли элемент на странице
        /// </summary>
        public bool Exists(string scssSelector) {
            return Exists(ScssBuilder.CreateBy(scssSelector));
        }

        public bool Exists(By by) {
            return Browser.Find.ElementFastS(by, null, false) != null;
        }

        /// <summary>
        /// true - выполняется хотя бы один ajax запрос
        /// </summary>
        public bool AjaxActive() {
            return !Browser.Js.Excecute<bool>(@"
                        var isJqueryComplete = typeof(jQuery) != 'function' || !jQuery.active;
                        var isPrototypeComplete = typeof(Ajax) != 'function' || !Ajax.activeRequestCount;
                        var isDojoComplete = typeof(dojo) != 'function' || !dojo.io.XMLHTTPTransport.inFlight.length;
                        return isJqueryComplete && isPrototypeComplete && isDojoComplete;");
        }

        /// <summary>
        /// Отмечен ли чекбокс
        /// </summary>
        public bool Checked(string scssSelector) {
            return Checked(ScssBuilder.CreateBy(scssSelector));
        }

        public bool Checked(By by, By frameBy = null)
        {
            return RepeatAfterStale(() => Checked(Browser.Find.Element(by, frameBy, false)));
        }

        public bool Checked(IWebElement element) {
            return element.GetAttribute("checked") == "true";
        }
		


        public bool Expanded(IWebElement element)
        {
            return element.GetAttribute("aria-expanded") == "true";
        }
        
        public string GetUrl(By by)
        {
            return Browser.Find.Element(by).GetAttribute("src");
        }

		public bool Active(string scssSelector)
		{
			return Active(ScssBuilder.CreateBy(scssSelector));
		}

		public bool Active(By by, By frameBy = null)
		{
			return RepeatAfterStale(() => Active(Browser.Find.Element(by, frameBy, false)));
		}

		public bool Active(IWebElement element)
		{
			return !element.GetAttribute("class").Contains("disabled");
		}

        public bool Selected(By by, By frameBy) {
            return Browser.Get.Attr(by, frameBy, "selected") != null;
        }
    }
}