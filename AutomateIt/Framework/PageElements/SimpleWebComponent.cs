using automateit.SCSS;
using OpenQA.Selenium;

using selenium.core.Framework.Page;
using selenium.core.SCSS;

namespace selenium.core.Framework.PageElements {
	public abstract class SimpleWebComponent : ContainerBase {
		public readonly By By;

		protected SimpleWebComponent(IPage parent, By by)
			: base(parent) {
			By = by;
		}

		protected SimpleWebComponent(IPage parent, string rootScss)
			: base(parent, rootScss) {
				By = ScssBuilder.CreateBy(rootScss);
		}
		

		public override bool IsVisible() {
	        return Is.Visible(By, FrameBy);
	    }
	}

	public abstract class WebElementWrap : ComponentBase {
		public readonly IWebElement Element;

		protected WebElementWrap(IPage parent, IWebElement element)
			: base(parent) {
			Element = element;
		}

		public override bool IsVisible() {
			return Element.Displayed;
		}

		public virtual void ClickAndWaitWhileAjax(bool ajaxInevitable=false) {
			Action.ClickAndWaitWhileAjax(Element, ajaxInevitable: ajaxInevitable);
		}
	}
}