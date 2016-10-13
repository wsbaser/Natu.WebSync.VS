namespace selenium.core.Framework.PageElements {
	public sealed class SimpleWebComponentAttribute : FindsByAttribute, IComponentAttribute {
		private string _componentName;

		public string ComponentName {
			get {
				return string.IsNullOrEmpty(_componentName) && !string.IsNullOrEmpty(LinkText)
						   ? LinkText
						   : _componentName;
			}
			set { _componentName = value; }
		}

		public string FrameScss { get; set; }

		#region IComponentArgs Members

		public object[] Args {
			get { return new object[] {Finder}; }
		}

		#endregion
	}
}