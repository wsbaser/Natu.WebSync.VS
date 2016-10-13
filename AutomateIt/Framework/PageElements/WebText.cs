using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;

using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
	public class WebText : SimpleWebComponent {
		public WebText(IPage parent, By by)
			: base(parent, by)
		{
		}

		public WebText(IPage parent, string rootScss)
			: base(parent, rootScss) {
		}

	    public override string Text {
	        get { return Get.Text(By, FrameBy); }
	    }

	    public string TextOrNull => IsVisible() ? Get.Text(By, FrameBy) : null;


	    public void AssertIsVisible() {
			Assert.IsTrue(IsVisible(), $"{ComponentName} is not displayed");
		}

		public void AssertIsHidden() {
			Assert.IsFalse(IsVisible(), $"{ComponentName} is displayed");
		}

		public void Click(int sleepTimeout=0) {
			Log.Action("Click on fake link '{0}'", ComponentName);
			Action.ClickAndWaitWhileAjax(By, FrameBy, sleepTimeout);
		}

		public void ClickAndWaitWhileAjaxRequests() {
			Log.Action("Click on fake link '{0}'", ComponentName);
			Action.ClickAndWaitWhileAjax(By, FrameBy);
		}

		public void ClickAndWaitForRedirect(bool waitForAjax = false, bool ajaxInevitable = false) {
			Log.Action("Click on fake link '{0}'", ComponentName);
			Action.ClickAndWaitForRedirect(By, FrameBy, waitForAjax, ajaxInevitable);
		}

		public void ClickAndWaitForState(Func<bool> checkState)
		{
			Log.Action("Click on fake link '{0}' with waiting", ComponentName);
			Action.ClickAndWaitForState(By,checkState);
		}

		public T Value<T>() {
			return Get.Value<T>(By);
		}

		public void DragByHorizontal(int pixels) {
			Log.Action("Drag '{0}' at {1} pixels", ComponentName, pixels);
			Action.DragByHorizontal(By, pixels);
		}

		public bool IsExist()
		{
			return IsExist();
		}

		public void MouseOver()
		{
			Action.MouseOver(By, FrameBy);
		}
	}
}