namespace selenium.core.Framework.PageElements {
	public interface IComponentAttribute {
		object[] Args { get; }
		string ComponentName { get; set; }
		string FrameScss { get; set; }
	}
}