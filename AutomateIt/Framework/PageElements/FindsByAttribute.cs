using System;
using automateit.SCSS;
using OpenQA.Selenium;

using selenium.core.SCSS;

namespace selenium.core.Framework.PageElements {
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public class FindsByAttribute : Attribute {
		public string Name { get; set; }
		public string TagName { get; set; }
		public string Css { get; set; }
		public string ClassName { get; set; }
		public string ID { get; set; }
		public string XPath { get; set; }
		public string LinkText { get; set; }
		public string PartialLinkText { get; set; }
		public string Scss { get; set; }

		private By _finder;

		internal By Finder {
			get { return _finder ?? (_finder = CreateFinder()); }
		}

		private By CreateFinder() {
			By by = null;
			if (SelectorNotEmpty(Name, by))
				by = By.Name(Name);
			if (SelectorNotEmpty(TagName, by))
				by = By.TagName(TagName);
			if (SelectorNotEmpty(Css, by))
				by = By.CssSelector(Css);
			if (SelectorNotEmpty(ClassName, by))
				by = By.ClassName(ClassName);
			if (SelectorNotEmpty(ID, by))
				by = By.Id(ID);
			if (SelectorNotEmpty(XPath, by))
				by = By.XPath(XPath);
			if (SelectorNotEmpty(LinkText, by))
				by = By.LinkText(LinkText);
			if (SelectorNotEmpty(PartialLinkText, by))
				by = By.PartialLinkText(PartialLinkText);
			if (SelectorNotEmpty(Scss, by))
				by = ScssBuilder.CreateBy(Scss);
			if (by == null)
				throw new Exception("No one selector is defined");
			return by;
		}

		private bool SelectorNotEmpty(string value, By by) {
			if (string.IsNullOrEmpty(value))
				return false;
			if (by != null)
				throw new Exception("More than one selector defined");
			return true;
		}
	}
}