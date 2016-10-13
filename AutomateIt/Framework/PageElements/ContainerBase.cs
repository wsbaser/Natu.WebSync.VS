using automateit.SCSS;
using OpenQA.Selenium;
using selenium.core.Framework.Page;
using selenium.core.SCSS;

namespace selenium.core.Framework.PageElements {
	public abstract class ContainerBase : ComponentBase, IContainer {
		private string _rootScss;

		protected virtual string RootScss {
			get { return _rootScss ?? (_rootScss = "html"); }
		}

		protected By RootSelector {
			get { return ScssBuilder.CreateBy(RootScss); }
		}

		protected ContainerBase(IPage parent)
			: this(parent, null) {
		}

		protected ContainerBase(IPage parent, string rootScss)
			: base(parent) {
			_rootScss = rootScss;
		}

		public override bool IsVisible() {
			return Is.Visible(RootSelector, FrameBy);
		}

		/// <summary>
		/// �������� Scss ��� ���������� ��������
		/// </summary>
		public string InnerScss(string relativeScss, params object[] args) {
			relativeScss = string.Format(relativeScss, args);
			return ScssBuilder.Concat(RootScss, relativeScss).Value;
		}

		/// <summary>
		/// �������� �������� ��� ���������� ��������
		/// </summary>
		public By InnerSelector(string relativeScss, params object[] args) {
			relativeScss = string.Format(relativeScss, args);
			return ScssBuilder.Concat(RootScss, relativeScss).By;
		}

		/// <summary>
		/// �������� �������� ��� ���������� ��������
		/// </summary>
		public By InnerSelector(Scss innerScss) {
			Scss rootScss = ScssBuilder.Create(RootScss);
			return rootScss.Concat(innerScss).By;
		}
	}
}