using automateit.SCSS;
using selenium.core.Framework.Page;
using selenium.core.Framework.PageElements;
using selenium.core.SCSS;

namespace automateit.Framework.PageElements
{
	public abstract class WebLookup: ContainerBase
	{
		public WebLookup(IPage parent, string rootScss) : base(parent, rootScss)
		{
		}
	}
}
