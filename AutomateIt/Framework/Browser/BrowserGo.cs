/**
* Created by VolkovA on 27.02.14.
*/ // Методы для навигации по страницам

using System;
using System.IO;
using System.Security.Policy;
using System.Web;
using automateit.Auxiliary;
using OpenQA.Selenium;
using selenium.core.Auxiliary;
using selenium.core.Exceptions;
using selenium.core.Framework.Page;
using selenium.core.Framework.Service;

namespace selenium.core.Framework.Browser {
	public class BrowserGo : DriverFacade {
		public BrowserGo(Browser browser)
			: base(browser) {
		}

		// Определение Url, соответствующее классу страницы и переход на него
		public T ToPage<T>() where T : IPage {
			var pageInstance = (T) Activator.CreateInstance(typeof (T));
			ToPage(pageInstance);
		    return Browser.State.PageAs<T>();
		}

		// Определение Url, соответствующее классу страницы и переход на него
		public void ToPage(IPage page) {
			RequestData requestData = Web.GetRequestData(page);
			ToUrl(requestData);
		}

		public void ToUrl(String url) {
			ToUrl(new RequestData(url));
		}

		// Переход на указанный Url в текущем окне браузера
	    public void ToUrl(RequestData requestData) {
	        if (requestData.HasBasicAuth()) {
	            Log.Action("Navigating to url: {0} with Basic Authentication", requestData.Url);
	            string basicAuthDomain =
	                $"{HttpUtility.UrlEncode(requestData.BasicAuthLogin)}:{HttpUtility.UrlEncode(requestData.BasicAuthPassword)}@{requestData.Url.Host}";
	            string url = $"{requestData.Url.Scheme}://{basicAuthDomain}{requestData.Url.PathAndQuery}";
	            Driver.Navigate().GoToUrl(url);
	        }
	        else {
	            Log.Action("Navigating to url: {0}", requestData.Url);
	            Driver.Navigate().GoToUrl(requestData.Url);
	        }
	        Browser.State.Actualize();
	        Browser.Wait.WhileAjax();
	    }

	    /// <summary>
		/// Сохранить исходный код страницы на диске и открыть страницу в браузере
		/// </summary>
		/// <typeparam name="T">Класс страницы</typeparam>
		/// <param name="html">Исходный код страницы</param>
		public T ToHtml<T>(string html) where T : IPage {
			// Сохраниить на диск
			var type = typeof (T);
			string fileName = type.Name + ".html";
			string pagesFolder = Path.Combine(Environment.CurrentDirectory, "SavedPages");
			if (!Directory.Exists(pagesFolder))
				Directory.CreateDirectory(pagesFolder);
			string filePath = Path.Combine(pagesFolder, fileName);
			File.WriteAllText(filePath, html);

			// Открыть в браузере
			ToUrl("file://" + filePath);

			// Создать соответствующий класс страницы
			var page = (T) Activator.CreateInstance(type);
			page.Activate(Browser, Log);
			return page;
		}

		/// <summary>
		/// Найти письмо с указанным заголовком на указанном почтовом ящике.
		/// Открыть текст письма в браузере
		/// </summary>
//		public P ToEmail<P>(string email, string titlePattern) where P : IPage {
//			string messageBody = MailHelper.GetMessage(email, titlePattern);
//			if (string.IsNullOrEmpty(messageBody))
//				Throw.FrameworkException("На '{0}' не пришло письмо активации");
//			return Browser.Go.ToHtml<P>(messageBody);
//		}

		/// <summary>
		/// Вернуться на предыдущую страницу
		/// </summary>
		public void Back() {
            Driver.Navigate().Back();
            Log.Action("Go.Back(). Result Url: {0}", Driver.Url);
			Browser.State.Actualize();
		}

	    public void Refresh() {
	        Log.Action("Refresh page {0}", Driver.Url);
	        Browser.State.Actualize();
	    }
	}
}