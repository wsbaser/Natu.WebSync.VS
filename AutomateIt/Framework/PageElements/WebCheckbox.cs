using NUnit.Framework;
using OpenQA.Selenium;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
	public class WebCheckbox : SimpleWebComponent, IWebCheckbox {
		public WebCheckbox(IPage parent, By by)
			: base(parent, by) {
		}

		public WebCheckbox(IPage parent, string rootScss)
			: base(parent, rootScss) {
		}
		
		public void Check() {
			if (!Is.Checked(By,FrameBy)) {
				Log.Action("Check {0} checkbox ", ComponentName);
				Action.Click(By,FrameBy);
			}
		}
		
		public void Uncheck() {
			if (Checked()) {
				Log.Action("Uncheck checkbox {0}", ComponentName);
				Action.Click(By,FrameBy);
			}
		}

		public bool Checked() {
			return Is.Checked(By, FrameBy);
		}

		public void AssertIsDisabled() {
			Assert.AreEqual("disabled", Get.Attr(By, "disabled"), "Checkbox is active");
		}
		public void AssertIsUnchecked()
		{
			Assert.IsFalse(Checked(), "Checkbox is checked");
		}
	}
	public interface IWebCheckbox{
		void Check();
		void Uncheck();
		bool Checked();
	}
}