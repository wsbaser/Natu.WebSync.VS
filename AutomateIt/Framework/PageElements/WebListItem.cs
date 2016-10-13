using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
    public class WebListItem : ItemBase {
		public override string ItemScss => ContainerInnerScss("li[.='{0}']", ID);

		public WebListItem(IContainer container, string id) : base(container, id) {
        }

        public void ClickAndWatiForRedirect(bool waitForAjax = false, bool ajaxInevitable = false) {
            Log.Action("Click by list item '{0}'", ID);
            Action.ClickAndWaitForRedirect(ItemScss, FrameBy, waitForAjax, ajaxInevitable);
        }

	    public void Click()
		{
			Log.Action("Click by list item '{0}'", ID);
			Action.Click(ItemScss, FrameBy);
		}

		public void MouseOver()
		{
			Log.Action("Click by list item '{0}'", ID);
			Action.MouseOver(ItemScss, FrameBy);
		}

		public bool ContainsLink() {
            return Is.Exists(InnerScss(">a"));
        }
	}
}