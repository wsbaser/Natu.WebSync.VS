using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO {
    public class PageDto : EmberDtoBase
    {
        public PageDto(IPage page) : base(page.Id) {
        }
    }
}