/**
* Created by VolkovA on 27.02.14.
*/ // Методы для выполнения действий со страницей

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using automateit.SCSS;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using selenium.core.Exceptions;
using selenium.core.SCSS;

namespace selenium.core.Framework.Browser {
	public class BrowserAction : DriverFacade {
		private readonly BrowserFind _find;

		public BrowserAction(Browser browser)
			: base(browser) {
			_find = browser.Find;
		}
		
		/// <summary>
		/// Выбрать опцию в html теге Select
		/// </summary>
		public void Select(string scssSelector, String value) {
			Select(ScssBuilder.CreateBy(scssSelector), value);
		}

		public void Select(By by, String value) {
			RepeatAfterStale(
				() => {
					IWebElement select = _find.Element(by);
					var dropDown = new SelectElement(select);
					dropDown.SelectByValue(value);
				});
		}


		/// <summary>
		/// Ввести значение в поле ввода
		/// </summary>
		public void TypeIn(string scssSelector, By frameBy, Object value, bool clear = true)
		{
			TypeIn(ScssBuilder.CreateBy(scssSelector), frameBy, value, clear);
		}

		public void ClickBackButton()
		{
			string oldUrl = Browser.Window.Url;
			Driver.Navigate().Back();
			for (int i = 0; i < 10; i++)
			{
				Browser.State.Actualize();
				if (Browser.State.SystemAlert != null || oldUrl != Browser.Window.Url)
				{
					Browser.Wait.WhileAjax();
					return;
				}
				Thread.Sleep(200);
			}
		}

		public void ClickNextButton()
		{
			string oldUrl = Browser.Window.Url;
			Driver.Navigate().Forward();
			for (int i = 0; i < 10; i++)
			{
				Browser.State.Actualize();
				if (Browser.State.SystemAlert != null || oldUrl != Browser.Window.Url)
				{
					Browser.Wait.WhileAjax();
					return;
				}
				Thread.Sleep(200);
			}
		}

		public void TypeIn(By by, By frameBy, Object value, bool clear = true) {
			RepeatAfterStale(
				() => {
					IWebElement element = _find.Element(by,frameBy);
					if (clear)
						Clear(element);
					foreach (char c in value.ToString())
						element.SendKeys(c.ToString());
				});
		}

	    /// <summary>
	    /// Клик по элементу
	    /// </summary>
	    public void Click(string scssSelector, By frameBy = null, int sleepTimeout = 0) {
	        Click(ScssBuilder.CreateBy(scssSelector), frameBy, sleepTimeout);
	    }

	    public void Click(By by, By frameBy = null, int sleepTimeout = 0) {
	        Click(Driver, by, frameBy, sleepTimeout);
	    }

	    public void Click(ISearchContext context, By by, By frameBy, int sleepTimeout = 0) {
	        RepeatAfterStale(
	            () => {
	                try {
	                    if (Browser.Options.WaitWhileAjaxBeforeClick)
	                        Browser.Wait.WhileAjax();
	                    Click(_find.Element(context, by, frameBy), sleepTimeout);
	                }
	                catch (InvalidOperationException e) {
	                    Log.Selector(by);
	                    Log.Exception(e);
	                }
	            });
	    }

	    public void Click(IWebElement element, int sleepTimeout = 0) {
			Browser.Js.ScrollIntoView(element); // Fix for "element not visible" exception
			element.Click();
			if (sleepTimeout != 0)
				Thread.Sleep(sleepTimeout);
		}

	    /// <summary>
	    /// Клик по элементу с ожиданием алерта
	    /// </summary>
	    public void ClickAndWaitForAlert(string scssSelector, int timeout = BrowserTimeouts.AJAX) {
	        ClickAndWaitForAlert(ScssBuilder.CreateBy(scssSelector), null, timeout);
	    }

	    public void ClickAndWaitForAlert(By by, By frameBy, int timeout = BrowserTimeouts.AJAX) {
			Click(by,frameBy);
            Browser.Wait.ForAlert(timeout);
		}

		/// <summary>
		/// Клик и ожидание редиректа
		/// </summary>
		public void ClickAndWaitForRedirect(string scssSelector, By frameBy, bool waitForAjax = false, bool ajaxInevitable = false) {
			ClickAndWaitForRedirect(ScssBuilder.CreateBy(scssSelector), frameBy, waitForAjax, ajaxInevitable);
		}

		public void ClickAndWaitForRedirect(By by, By frameBy, bool waitForAjax = false, bool ajaxInevitable = false) {
			RepeatAfterStale(() => ClickAndWaitForRedirect(Browser.Find.Element(by, frameBy), waitForAjax, ajaxInevitable));
		}

	    public void ClickAndWaitForRedirect(IWebElement element, bool waitForAjax = false, bool ajaxInevitable = false) {
	        string oldUrl = Browser.Window.Url;
	        Click(element, 1000);
	        Browser.Wait.ForRedirect(oldUrl, waitForAjax, ajaxInevitable);
	    }

	    public void ClickAndWaitForState(string scssSelector, Func<bool> checkState)
		{
			ClickAndWaitForState(ScssBuilder.CreateBy(scssSelector), checkState);
		}

		public void ClickAndWaitForState(By by, Func<bool> checkState)
		{
			RepeatAfterStale(() => ClickAndWaitForState(Browser.Find.Element(by), checkState));
		}

		public void ClickAndWaitForState(IWebElement element, Func<bool> checkState)
		{
			Click(element);
			try
			{
				Browser.Wait.Until(checkState);
			}
			catch (WebDriverTimeoutException)
			{
				Log.Info("Waited state not appeared");
			}
		}

		/// <summary>
		/// Клик и ожидание пока отрыбатывают Ajax Запросы
		/// </summary>
		public void ClickAndWaitWhileAjax(string scssSelector, int sleepTimeout = 0, bool ajaxInevitable = false) {
			ClickAndWaitWhileAjax(ScssBuilder.CreateBy(scssSelector),null, sleepTimeout, ajaxInevitable);
		}

		public void ClickAndWaitWhileAjax(By by, By frameBy, int sleepTimeout = 0, bool ajaxInevitable = false) {
			ClickAndWaitWhileAjax(Driver, by,frameBy, sleepTimeout, ajaxInevitable);
		}

	    public void ClickAndWaitWhileAjax(ISearchContext context, By by, By frameBy, int sleepTimeout = 0,
	        bool ajaxInevitable = false) {
	        RepeatAfterStale(
	            () => ClickAndWaitWhileAjax(Browser.Find.Element(context, by, frameBy), sleepTimeout, ajaxInevitable));
	    }

	    public void ClickAndWaitWhileAjax(IWebElement element, int sleepTimeout = 0, bool ajaxInevitable = false) {
			Browser.Wait.WhileAjax();
			Click(element, sleepTimeout);
			Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
			Browser.State.Actualize();
		}

		/// <summary>
		/// Нажатие клавиши в поле ввода
		/// </summary>
		public void PressKey(string scssSelector, string key) {
			PressKey(ScssBuilder.CreateBy(scssSelector), key);
		}

		public void PressKey(By by, string key)
		{
			PressKey(by, null, key);
		}

		public void PressKey(By by, By frameBy, string key)
		{
			RepeatAfterStale(() => PressKey(Browser.Find.Element(by, frameBy), key));
		}

		public void PressKey(IWebElement element, string key) {
			element.SendKeys(key);
		}

		/// <summary>
		/// Очистить текстовое поле
		/// </summary>
		public void Clear(string scssSelector) {
			Clear(ScssBuilder.CreateBy(scssSelector));
		}

		public void Clear(By by, By frameBy=null) {
			RepeatAfterStale(() => Clear(Browser.Find.Element(by,frameBy)));
		}

		public void Clear(IWebElement element) {
			element.Clear();
		}

		/// <summary>
		/// Убрать фокус с текущего компонента
		/// </summary>
		public void ChangeFocus() {
			PressKey(Driver.SwitchTo().ActiveElement(), Keys.Tab);
		}

		/// <summary>
		/// Switch to frame
		/// </summary>
		public void SwitchToFrame(By by, By frameBy)
		{
			IWebElement frame = _find.Element(by, frameBy);
			Driver.SwitchTo().Frame(frame);
		}
		
		/// <summary>
		/// Switch to default content
		/// </summary>
		public void SwitchToDefaultContent()
		{
			Driver.SwitchTo().DefaultContent();
		}

		/// <summary>
		/// Кликнуть и подождать пока на странице отображается прогресс
		/// </summary>
		/// <param name="sleepTimeout">принудительное ожидание после выполнения клика</param>
		/// <param name="progressInevitable">
		/// true означает что после клика прогресс точно должен появиться
		/// поэтому сначала ожидаем его появления, потом ожидаем пока он не исчезнет
		/// </param>
		public void ClickAndWaitWhileProgress(string scssSelector, int sleepTimeout = 0, bool progressInevitable = false) {
			ClickAndWaitWhileProgress(ScssBuilder.CreateBy(scssSelector),null, sleepTimeout, progressInevitable);
		}

		public void ClickAndWaitWhileProgress(By by, By frameBy, int sleepTimeout = 0, bool progressInevitable = false) {
			Click(by, frameBy, sleepTimeout);
			if (progressInevitable)
				Browser.Wait.ForPageProgress();
			Browser.Wait.WhilePageInProgress();
			Browser.State.Actualize();
		}

		/// <summary>
		/// Нажать Enter в поле найденному по селектору
		/// </summary>
		public void PressEnter(string scssSelector) {
			PressEnter(ScssBuilder.CreateBy(scssSelector));
		}

		public void PressEnter(By by)
		{
			PressEnter(by, null);
		}

		public void PressEnter(By by, By frameBy) {
			PressKey(by, frameBy, Keys.Enter);
		}

		/// <summary>
		/// Клик по всем элементам найденным по указанному селектору
		/// </summary>
		public void ClickByAll(string scssSelector) {
			ClickByAll(ScssBuilder.CreateBy(scssSelector));
		}

		public void ClickByAll(By by) {
			List<IWebElement> elements = Browser.Find.Elements(by);
			foreach (IWebElement element in elements)
				Browser.Action.Click(element);
		}

		/// <summary>
		/// Прокрутить страницу до низа
		/// </summary>
		public void ScrollToBottom() {
			DateTime start = DateTime.Now;
			do {
				Browser.Js.ScrollToBottom();
				Browser.Wait.WhileAjax(ajaxInevitable: true);
			} while (!Browser.Js.IsPageBottom() && (DateTime.Now - start).TotalSeconds < 300);
		}

		/// <summary>
		/// Сохранить
		/// </summary>
		/// <param name="marker">название файла скриншота</param>
		/// <param name="folder">папка для скриншотов</param>
		public void SaveScreenshot(string marker = null, string folder = "d:\\") {
			Bitmap screenshot = Browser.Get.Screenshot();
			string filename = string.IsNullOrEmpty(marker) ? new Random().Next(100000).ToString() : marker;
			string screenshotFilePath = Path.Combine(folder, filename + ".png");
			screenshot.Save(screenshotFilePath, ImageFormat.Png);
			Console.WriteLine("Screenshot: {0}", new Uri(screenshotFilePath));
		}

		/// <summary>
		/// Навести курсор на элемент
		/// </summary>
		public void MouseOver(string scssSelector, By frameBy, int sleepTimeout = 0) {
			MouseOver(ScssBuilder.CreateBy(scssSelector), frameBy, sleepTimeout);
		}

		public void MouseOver(By by, By frameBy, int sleepTimeout = 0) {
			var action = new Actions(Driver);
			RepeatAfterStale(
				() => {
					IWebElement element = Browser.Find.Element(by, frameBy);
					action.MoveToElement(element).Build().Perform();
					if (sleepTimeout != 0)
						Thread.Sleep(sleepTimeout);
				});
		}

		public void SetFocus(IWebElement element) {
			if (element.TagName == "input")
				element.SendKeys("");
			else
				new Actions(Driver).MoveToElement(element).Build().Perform();
		}

		/// <summary>
		/// Перетащить элемент на указанное количество пикселей по горизонтали
		/// </summary>
		public void DragByHorizontal(string scssSelector, int pixels) {
			DragByHorizontal(ScssBuilder.CreateBy(scssSelector), pixels);
		}

		public void DragByHorizontal(By by, int pixels) {
			var builder = new Actions(Driver);
			builder.DragAndDropToOffset(Browser.Find.Element(by), pixels, 0).Build().Perform();
		}
	}
}