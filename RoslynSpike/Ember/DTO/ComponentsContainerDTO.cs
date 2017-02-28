using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO
{
    public class ComponentsContainerDTO:EmberDtoBase
    {
        public ComponentsContainerDTO(IComponentsContainer container):base(container.Id) {
        }
    }
}