using Microsoft.VisualStudio.TestTools.UnitTesting;

using automateit.SCSS;
using OpenQA.Selenium;

using selenium.core.Framework.Page;
using selenium.core.SCSS;

namespace selenium.core.Framework.PageElements 
{
	public class WebTabs : SimpleWebComponent
	{
		public string ItemScss { get; private set; }
		public string SelectedClass { get; private set; }

		public WebTabs(IPage parent, By @by)
			:  base(parent, @by)
		{ 
		}

		public WebTabs(IPage parent, string rootScss, string itemScss, string selectedClass)
			: base(parent, rootScss) {
			ItemScss = itemScss;
			SelectedClass = selectedClass;
		}

		public void SelectTab(string tabName)
		{
			if (!IsActive(tabName))
				ClickTab(tabName);
		}

		public void ClickTab(string tabName)
		{
			Action.Click(TabSelector(tabName),FrameBy);
		}

		public bool IsActive(string tabName)
		{
			return Is.HasClass(TabSelector(tabName), FrameBy, SelectedClass);
		}

		public bool IsVisible(string tabName)
		{
			return Is.Visible(TabSelector(tabName));
		}

		public By TabSelector(string tabName)
		{
			return ScssBuilder.CreateBy(ItemScss, tabName);
		}

		public void SelectAndWaitWhileAjaxRequests(string tabName, int sleepTimeout = 0, bool ajaxInevitable = false)
		{
			if (!IsActive(tabName))
				Action.ClickAndWaitWhileAjax(TabSelector(tabName),FrameBy, sleepTimeout, ajaxInevitable);
		}

		public void AssertIsVisible(string tabName)
		{
			Assert.IsTrue(IsVisible(tabName), "{0} '{1}' tab is not displayed", ComponentName, tabName);
		}

		public void AssertNotVisible(string tabName)
		{
			Assert.IsFalse(IsVisible(tabName), "{0} '{1}' tab is displayed", ComponentName, tabName);
		}

		public void AssertIsActive(string tabName)
		{
			Assert.IsTrue(IsActive(tabName), "{0} '{1}' tab is not active", ComponentName, tabName);
		}

		public override string Text { get; }
	}
}

