using OpenQA.Selenium;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
    public class WebImage : SimpleWebComponent {
		public WebImage(IPage parent, By @by) : base(parent, @by) {
		}

		public WebImage(IPage parent, string rootScss)
			: base(parent, rootScss) {
		}
		
		public string GetFileName() {
			return Get.ImgFileName(By);
		}

        public void Click(int sleepTimeout = 0) {
            Log.Action("Click by icon '{0}'", ComponentName);
            Action.Click(By, FrameBy, sleepTimeout);
        }

		public void ClickAndWaitForRedirect(bool waitForAjax = false, bool ajaxInevitable = false)
		{
			Log.Action("Click '{0} button'", ComponentName);
			Action.ClickAndWaitForRedirect(By, FrameBy, waitForAjax, ajaxInevitable);
		}

		public void ClickAndWaitWhileAjax(int sleepTimeout = 0, bool ajaxInevitable = false)
		{
			Log.Action("Click '{0} button'", ComponentName);
			Action.ClickAndWaitWhileAjax(By, FrameBy, sleepTimeout, ajaxInevitable);
		}
	}
}