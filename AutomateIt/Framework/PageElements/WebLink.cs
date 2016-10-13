using OpenQA.Selenium;

using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
	public class WebLink : SimpleWebComponent, IClickable {
		public WebLink(IPage parent, By @by)
			: base(parent, @by) {
		}

	    public WebLink(IPage parent, string rootScss) : base(parent, rootScss) {
	    }

	    public string AnchorText {
	        get { return Get.Text(By, FrameBy); }
	    }

	    #region IClickable Members
		
		public void Click(int sleepTimeout=0) {
			Log.Action("Click '{0}' link", ComponentName);
			Action.ClickAndWaitForRedirect(By, FrameBy);
		}

		#endregion
		
		public void ClickAndWaitWhileAjax(bool ajaxInevitable=false) {
			Log.Action("Click '{0}' link", ComponentName);
			Action.ClickAndWaitWhileAjax(By, FrameBy, ajaxInevitable: ajaxInevitable);
		}

		public void ClickAndWatiForRedirect(bool waitForAjax = false, bool ajaxInevitable = false)
		{
			Log.Action("Click '{0}' link", ComponentName);
			Action.ClickAndWaitForRedirect(By, FrameBy, waitForAjax, ajaxInevitable);
		}
		
		public void MouseOver(int sleepTimeout = 0) {
			Log.Action("Hover cursor on '{0}' link", ComponentName);
			Action.MouseOver(By,FrameBy, sleepTimeout);
		}
	}
}