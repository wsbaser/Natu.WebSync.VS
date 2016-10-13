using System.Collections.Generic;
using automateit.SCSS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;

using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
	public abstract class DropListBase : ContainerBase, IDropList {
		public DropListBase(IPage parent, string rootScss)
			: base(parent, rootScss) {
		}

		public abstract string GetItemScss(string name);

	    public By GetItemSelector(string name) {
	        return ScssBuilder.CreateBy(GetItemScss(name));
	    }

		public abstract string ItemNameScss { get; }

		public List<string> GetItems() {
			return Get.Texts(ItemNameScss);
		}

	    /// <summary>
	    /// Select option if it not selected yet
	    /// </summary>
	    /// <param name="name">Option text</param>
	    /// <returns>true if selection excecuted</returns>
	    public virtual bool SelectOption(string name) {
	        if (IsOptionSelected(name)) {
	            return false;
	        }
	        Action.Click(GetItemScss(name), FrameBy);
	        return true;
	    }

	    protected virtual bool IsOptionSelected(string name) {
	        return false;
	    }

	    public void SelectOptionAndWaitWhileAjax(string name) {
	        Action.ClickAndWaitWhileAjax(GetItemSelector(name), FrameBy);
	    }

	    public void AssertContains(string item) {
			Assert.IsTrue(Contains(item));
		}
		
		/// <summary>
		/// Содержит ли список указанное значение
		/// </summary>
		public bool Contains(string item) {
			return GetItems().Contains(item);
		}
	}
}