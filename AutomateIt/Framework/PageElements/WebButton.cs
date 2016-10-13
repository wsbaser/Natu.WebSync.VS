using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;

using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements
{
    public class WebButton : SimpleWebComponent, IClickable
    {
        public WebButton(IPage parent, By by)
			: base(parent, by) {
        }

        public WebButton(IPage parent, string rootScss) : base(parent, rootScss)
        {
        }

        #region IClickable Members

        public void Click(int sleepTimeout = 0)
        {
            Log.Action("Click '{0} button'", ComponentName);
            Action.Click(By,FrameBy, sleepTimeout);
        }

        #endregion

        public void ClickAndWaitForAlert()
        {
            Log.Action("Click '{0} button'", ComponentName);
            Action.ClickAndWaitForAlert(By,FrameBy);
        }

        public void ClickAndWaitWhileAjax(int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            Log.Action("Click '{0} button'", ComponentName);
            Action.ClickAndWaitWhileAjax(By,FrameBy, sleepTimeout, ajaxInevitable);
        }

        public void ClickAndWaitForRedirect(bool waitForAjax = false, bool ajaxInevitable = false)
        {
            Log.Action("Click '{0} button'", ComponentName);
            Action.ClickAndWaitForRedirect(By,FrameBy, waitForAjax, ajaxInevitable);
        }

        public void ClickAndWaitWhileProgress() {
            Log.Action("Click '{0} button'", ComponentName);
            Action.ClickAndWaitWhileProgress(By, FrameBy, 1000);
        }

        public bool Active()
		{
			return Is.Active(By, FrameBy);
		}
		
		public override string Text
		{
			get
			{
				Log.Action("Get '{0}' title", ComponentName);
				return Get.Text(By,FrameBy);
			}
		}

		public void AssertIsVisible()
		{
			Assert.IsTrue(IsVisible(), "{0} is not displayed", ComponentName);
		}

		public void AssertIsNotVisible()
		{
			Assert.IsFalse(IsVisible(), "{0} is not displayed", ComponentName);
		}

		public void AssertIsActive()
		{
			Assert.IsTrue(Active(), "{0} is not active", ComponentName);
		}
	}
}