using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
	public abstract class ItemBase : ContainerBase, IItem {
		protected readonly IContainer Container;

		protected ItemBase(IContainer container, string id)
			: base(container.ParentPage) {
			Container = container;
			ID = id;
		}

		protected override string RootScss {
			get { return ItemScss; }
		}

		#region IItem Members

		public string ID { get; protected set; }
		public abstract string ItemScss { get; }

		#endregion

		protected string ContainerInnerScss(string relativeXpath, params object[] args) {
			return Container.InnerScss(relativeXpath, args);
		}
	}
}