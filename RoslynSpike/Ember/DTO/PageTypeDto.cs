using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO {
    public class PageTypeDto : ComponentsContainerDTO
    {
        public string basePageType;
        public string absolutePath;

        public PageTypeDto(IPageType page) : base(page)
        {
            basePageType = page.BasePageTypeId;
            absolutePath = page.AbsolutePath;
        }
    }
}