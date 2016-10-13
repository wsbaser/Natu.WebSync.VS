using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;

using selenium.core.Framework.Browser;
using selenium.core.Framework.Page;
using selenium.core.TestData;

namespace selenium.core.Framework.PageElements {
	public class WebInput : SimpleWebComponent {
		public WebInput(IPage parent, By @by)
			: base(parent, @by) {
		}
		public string Placeholder
		{
			get { return Get.Attr(By, FrameBy, "placeholder"); }
		}

		public WebInput(IPage parent, string rootScss)
			: base(parent, rootScss) {
		}

		public virtual string Text {
			get { return Get.InputValue(By, FrameBy); }
		}
		
		public virtual WebInput TypeIn(object value, bool clear = true) {
			Log.Action("Input '{0}' in '{1}' field", value, ComponentName);
			Action.TypeIn(By, FrameBy, value, clear:clear);
            return this;
		}
		
		public void AssertIsEmpty() {
			Assert.IsTrue(IsEmpty(), "Field '{0}' is not empty", ComponentName);
		}
		
		public void AssertPlaceholderEqual(string expected)
		{
			Assert.AreEqual(expected, Placeholder, $"Invalid placeholder in text field {ComponentName}");
		}

		public bool IsEmpty() {
			return string.IsNullOrWhiteSpace(Text);
		}
		
		public void AssertIsNotEmpty() {
			Assert.IsFalse(string.IsNullOrWhiteSpace(Text), $"Field '{ComponentName}' is empty");
		}
		
		public string TypeInRandomNumber(int length = 10) {
			var random = RandomDataHelper.Cifers(length);
			TypeIn(random);
			return random;
		}
		
		public void RemoveLast(bool changeFocus=false) {
			Action.PressKey(By, FrameBy, Keys.End);
			Action.PressKey(By, FrameBy, Keys.Backspace);
			if (changeFocus)
				Action.ChangeFocus();
		}
		
		public virtual void Clear() {
			Action.Clear(By,FrameBy);
		}

		public void TypeInAndSubmit(string query) {
			TypeIn(query);
			Action.PressEnter(By,FrameBy);
			Wait.WhileAjax();
		}

		public void SubmitAndWaitForRedirect(bool waitForAjax = false, bool ajaxInevitable = false)
        {
            Log.Action("Press enter in '{0}'", ComponentName);
            string oldUrl = Browser.Window.Url;
			Action.PressEnter(By, FrameBy);
            Browser.Wait.ForRedirect(oldUrl, waitForAjax, ajaxInevitable);
		}

	public void AssertEqual(string expected) {
			Assert.AreEqual(expected, Text, "Incorrect value in '{0}'", ComponentName);
			}
			
	public void SubmitAndWaitWhileAjax(bool ajaxInevitable = false) {
            Log.Action("Press enter in '{0}'", ComponentName);
            Action.PressEnter(By, FrameBy);
            Browser.Wait.WhileAjax(BrowserTimeouts.AJAX, ajaxInevitable);
		}
	}
}