using System;
using System.Collections.Generic;
using System.Linq;
using automateit.utils.Extensions;
using NUnit.Framework;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
	public abstract class ListBase<T> : ContainerBase, IWebList<T> where T : IItem, IComponent {
		public ListBase(IPage parent, string rootScss=null)
			: base(parent, rootScss) {
		}

		#region IWebList<T> Members

		public abstract string ItemIdScss { get; }

		public virtual List<string> GetIds() {
			return Get.Texts(ItemIdScss);
		}

		public virtual List<T> GetItems() {
			return GetIds().Select(id => (T) WebPageBuilder.CreateComponent<T>(this, id)).ToList();
		}

		#endregion

		public override bool IsVisible() {
			return Is.Visible(RootScss);
		}

		public T FindRandom(Func<T, bool> filter = null) {
			List<T> list = GetItems();
			list.Shuffle();
			return filter == null ? list.FirstOrDefault() : list.FirstOrDefault(filter);
		}

		public T FindSingle(Func<T, bool> filter) {
			List<T> list = GetItems();
			return list.SingleOrDefault(filter);
		}

		public T RandomItem() {
			return GetItems().RandomItem();
		}

		public List<T1> GetItems<T1>() {
			return GetItems().Cast<T1>().ToList();
		}

	    public T GetItem(string id) {
	        return (T) WebPageBuilder.CreateComponent<T>(this, id);
	    }

	    public void AssertNotEmpty() {
	        CollectionAssert.IsNotEmpty(GetIds(), $"'{ComponentName}' is empty");
	    }
	}
}