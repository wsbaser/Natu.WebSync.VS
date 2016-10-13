using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO {
    public class ComponentDto : EmberDtoBase
    {
        public ComponentDto(IComponent component) : base(component.Id) {
        }
    }
}