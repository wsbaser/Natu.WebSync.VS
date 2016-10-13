using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using automateit.SCSS;
using NUnit.Framework;
using OpenQA.Selenium;

using selenium.core.Framework.Page;
using selenium.core.Framework.PageElements;
using selenium.core.SCSS;

namespace automateit.Framework.PageElements
{
	public abstract class WebAccordionBase : SimpleWebComponent
	{
		public virtual void Expand()
		{
			if (!Expanded())
			{
				Log.Action("Expand Accordion {0}", ComponentName);
				Action.ClickAndWaitWhileAjax(By, FrameBy);
			}
		}

		public virtual void Collapse() {
			if (Expanded())
			{
				Log.Action("Collapse Accordion {0}", ComponentName);
				Action.Click(By, FrameBy);
			}
		}

		public virtual bool Expanded()
		{
			return Get.Attr(By, FrameBy, "aria-expanded") == "true";
		}
		public WebAccordionBase(IPage parent, string rootScss) : base(parent, rootScss)
		{
		}
	}
}
