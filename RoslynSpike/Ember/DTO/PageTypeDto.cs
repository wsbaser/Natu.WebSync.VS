using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO {
    public class PageTypeDto : ComponentsContainerDTO
    {
        public string BasePageType;
        public string AbsolutePath;

        public PageTypeDto(IPageType page) : base(page)
        {
            BasePageType = page.BasePageTypeId;
            AbsolutePath = page.AbsolutePath;
        }
    }
}