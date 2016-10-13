/**
* Created by VolkovA on 27.02.14.
*/ // Методы для поиска элементов на странице

using System;
using System.Collections.Generic;
using System.Linq;
using automateit.SCSS;
using OpenQA.Selenium;
using selenium.core.Exceptions;
using selenium.core.SCSS;

namespace selenium.core.Framework.Browser {
    public class BrowserFind : DriverFacade {
        public BrowserFind(Browser browser)
            : base(browser) {
        }

        /// <summary>
        /// Поиск элемента. Если не найден - кинуть исключение
        /// </summary>
        public IWebElement Element(string scssSelector) {
            return Element(ScssBuilder.CreateBy(scssSelector));
        }

        public IWebElement Element(By by, By frameBy = null, bool displayed = true) {
            return Element(Driver, by, frameBy, displayed);
        }

        public IWebElement Element(ISearchContext context, By by, bool displayed = true)
        {
            return Element(context, by, null, displayed);
        }

        public IWebElement Element(ISearchContext context, By by, By frameBy, bool displayed = true) {
            DateTime start = DateTime.Now;
            Browser.Driver.SwitchTo().DefaultContent();
            if (frameBy != null)
            {
                var frameElements = context.FindElements(frameBy);
                if (frameElements.Count == 0)
                {
                    Log.Selector(frameBy);
                    throw new NoSuchElementException($"Search time: {(DateTime.Now - start).TotalMilliseconds}");
                }
                if (frameElements.Count > 1)
                {
                    Log.Selector(frameBy);
                    Throw.TestException("Found more then 1 IFRAME element. Count: ", frameElements.Count);
                }
                else
                {
                    Browser.Driver.SwitchTo().Frame(frameElements[0]);
                }
            }
            start = DateTime.Now;
			List<IWebElement> elements = context.FindElements(by).ToList();
            if (elements.Count == 0) {
                Log.Selector(by);
                throw new NoSuchElementException($"Search time: {(DateTime.Now - start).TotalMilliseconds}");
            }
            if (displayed) {
                elements = elements.Where(e => e.Displayed).ToList();
                if (elements.Count == 0) {
	                try
	                {
						Browser.Wait.ForElementVisible(by, frameBy);
						elements = context.FindElements(by).Where(e => e.Displayed).ToList(); ;
					}
					catch (WebDriverTimeoutException)
	                {
						Log.Selector(by);
						throw new NoVisibleElementsException();
	                }
                }
            }
            if (elements.Count > 1)
            {
                Log.Selector(by);
                Throw.TestException("Found more then 1 element. Count: ", elements.Count);
            }
            return Browser.Options.FindSingle ? elements.SingleOrDefault() : elements.First();
        }

        /// <summary>
        /// Попытка поиска элемента. Если не найден - не кидать исключение
        /// </summary>
        public IWebElement ElementFastS(string scssSelector, bool displayed = true) {
            return ElementFastS(ScssBuilder.CreateBy(scssSelector), null, displayed);
        }

        public IWebElement ElementFastS(By by, By frameBy = null, bool displayed = true) {
            return ElementFastS(Driver, by, frameBy, displayed);
        }

        public IWebElement ElementFastS(ISearchContext context, By by, By frameBy, bool displayed = true) {
            try {
                Browser.DisableTimeout();
                return Element(context, by,frameBy, displayed);
            }
            catch (NoSuchElementException) {
                return null;
            }
            catch (NoVisibleElementsException) {
                return null;
            }
            finally {
                Browser.EnableTimeout();
            }
        }

        /// <summary>
        /// Найти элемент без ожидания
        /// </summary>
        public IWebElement ElementFast(string scssSelector) {
            return ElementFast(ScssBuilder.CreateBy(scssSelector));
        }

        public IWebElement ElementFast(By by) {
            try {
                Browser.DisableTimeout();
                Log.Selector(by);
                return Driver.FindElement(by);
            }
            finally {
                Browser.EnableTimeout();
            }
        }

        /// <summary>
        /// Найти элементы по указанному селектору без ожидания. Не падать если ничего не найдено
        /// </summary>
        public List<IWebElement> Elements(string scssSelector) {
            return Elements(ScssBuilder.CreateBy(scssSelector));
        }

        public List<IWebElement> Elements(By by, By frameBy = null) {
            return Elements(Driver, by, frameBy);
        }

        public List<IWebElement> Elements(ISearchContext context, By by, By frameBy=null) {
            try
            {
	            if (frameBy != null)
	            {
					var frameElements = context.FindElements(frameBy);
					Browser.Driver.SwitchTo().Frame(frameElements[0]);
				}
                //Browser.DisableTimeout();
                return new List<IWebElement>(context.FindElements(by));
            }
            catch (NoSuchElementException) {
                return new List<IWebElement>();
            }
            finally {
                //Browser.EnableTimeout();
            }
        }

        /// <summary>
        /// Найти элементы по указанному селектору без ожидания. Не падать если ничего не найдено
        /// Вернуть только видимые элементы
        /// </summary>
        public List<IWebElement> VisibleElements(string scssSelector) {
            return VisibleElements(ScssBuilder.CreateBy(scssSelector));
        }

        public List<IWebElement> VisibleElements(By by) {
            return RepeatAfterStale(() => Elements(by).Where(e => e.Displayed).ToList());
        }
    }
}