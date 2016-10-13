using System;
using System.Text.RegularExpressions;
using automateit.SCSS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;

using selenium.core.Exceptions;
using selenium.core.Framework.Browser;
using selenium.core.Logging;

namespace selenium.core.Framework.Page {
	public abstract class ComponentBase : IComponent {
		private string _componentName;

		protected ComponentBase(IPage parent) {
			ParentPage = parent;
		}

		#region IComponent Members

		public virtual string ComponentName {
			get {
				_componentName = _componentName ?? (_componentName = GetType().Name);
				return _componentName;
			}
			set { _componentName = value; }
		}

		public string FrameScss { get; set; }

		public By FrameBy => FrameScss != null ? ScssBuilder.CreateBy(FrameScss) : null;

		public IPage ParentPage { get; private set; }
		public abstract bool IsVisible();

		public virtual string Text { get; }

		public Browser.Browser Browser {
			get { return ParentPage.Browser; }
		}

		public TestLogger Log {
			get { return ParentPage.Log; }
		}

		public BrowserAction Action {
			get { return Browser.Action; }
		}

		public BrowserAlert Alert {
			get { return Browser.Alert; }
		}

		public BrowserFind Find {
			get { return Browser.Find; }
		}

		public BrowserGet Get {
			get { return Browser.Get; }
		}

		public BrowserGo Go {
			get { return Browser.Go; }
		}

		public BrowserIs Is {
			get { return Browser.Is; }
		}

		public BrowserState State {
			get { return Browser.State; }
		}

		public BrowserWait Wait {
			get { return Browser.Wait; }
		}

		public BrowserJs Js {
			get { return Browser.Js; }
		}

		public BrowserWindow Window {
			get { return Browser.Window; }
		}

		BrowserCookies IPageObject.Cookies {
			get { return Browser.Cookies; }
		}

		#endregion

		public void WaitForVisible() {
			Wait.Until(IsVisible);
		}

		public void AssertVisible() {
			Assert.IsTrue(IsVisible(), "'{0}' is not displayed", ComponentName);
		}

		public void AssertNotVisible()
		{
			Assert.IsFalse(IsVisible(), "'{0}' is displayed", ComponentName);
		}
		
		public void AssertNotEqual(string expected, bool ignoreRegister = false)
		{
			var text = ignoreRegister ? Text.ToLower() : Text;
			expected = ignoreRegister ? expected.ToLower() : expected;
			Assert.AreNotEqual(expected, text.Replace("'", ""), $"Component text '{ComponentName}' are not equal to expected value");
		}

		public void AssertEqual(string expected, bool ignoreRegister = false)
		{
			var text = ignoreRegister ? Text.ToLower() : Text;
			expected = ignoreRegister ? expected.ToLower() : expected;
			Assert.AreEqual(expected, text, $"Component text '{ComponentName}' are not equal to expected value");
		}

		public void AssertContains(string expected, bool ignoreRegister = false)
		{
			var text = ignoreRegister ? Text.ToLower().Trim() : Text;
			expected = ignoreRegister ? expected.ToLower().Trim() : expected;
			Assert.IsTrue(text.Contains(expected), $"Text in component {ComponentName} doesn't contain value '{expected}'");
		}

	    public void AssertMatch(string expectedPattern) {
	        var regex = new Regex(expectedPattern);
	        Assert.IsTrue(regex.IsMatch(Text), $"Text in component {ComponentName} doesn't match pattern '{expectedPattern}'");
	    }

	    public virtual ComponentBase Open(Action action)
		{
			if (!IsVisible())
				action.Invoke();
			try
			{
				Wait.Until(IsVisible);
			}
			catch (WebDriverTimeoutException)
			{
				throw Throw.FrameworkException("Component '{0}' is not opened", ComponentName);
			}
			return this;
		}

		public T Open<T>(Action action) where T:ComponentBase {
			return (T) Open(action);
		}
	}
}