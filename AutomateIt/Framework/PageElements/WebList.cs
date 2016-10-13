using System.Linq;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
    public class WebList : ListBase<WebListItem> {
        public WebList(IPage parent, string rootScss) : base(parent, rootScss) {
        }
		public override string ItemIdScss => InnerScss(">li");
    }
}