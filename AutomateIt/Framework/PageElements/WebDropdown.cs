using automateit.SCSS;
using NUnit.Framework;
using OpenQA.Selenium;
using selenium.core.Framework.Page;
using selenium.core.Framework.PageElements;

namespace automateit.Framework.PageElements
{
	public class WebDropdown : DropListBase
	{
		public override string ItemNameScss { get; }
		public string ItemScss { get; private set; }

		public WebDropdown(IPage parent, string rootScss, string itemScss) : base(parent, rootScss)
		{
			ItemScss = itemScss;
		}

		public override string GetItemScss(string option)
		{
			return InnerScss(ItemScss, option);
		}

		public override string Text
		{
			get { return Get.Text(InnerSelector("//span[@class='ng-binding ng-scope']"), FrameBy); }
		}

		public new void SelectOption(string option)
		{
			Action.Click(RootSelector, FrameBy);
			base.SelectOption(option);
		}

		public bool IdDropdownDisabled()
		{
			return Get.Attr(RootSelector, FrameBy, "class").Contains("disabled");
		}

		public void AssertWebDropdownIsDisabled()
		{
			Assert.IsTrue(IdDropdownDisabled(), $"Error: {ComponentName} is not disabled.");
		}

		public void AssertWebDropdownEnabled()
		{
			Assert.IsFalse(IdDropdownDisabled(), $"Error: {ComponentName} is disabled.");
		}
	}
}
