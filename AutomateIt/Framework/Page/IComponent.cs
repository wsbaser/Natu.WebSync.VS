/**
 * Created by VolkovA on 27.02.14.
 */

namespace selenium.core.Framework.Page {
    public interface IComponent : IPageObject {
        IPage ParentPage { get; }
        bool IsVisible();
        string ComponentName { get; set; }
        string FrameScss{ get; set; }
    }
}