namespace selenium.core.Framework.PageElements {
    public class WebLoaderArgsAttribute : FindsByAttribute, IComponentAttribute {
        #region IComponentArgs Members

        public object[] Args {
            get { return new object[] {Finder}; }
        }

        public string ComponentName { get; set; }
        public string FrameScss { get; set; }

        #endregion
    }
}