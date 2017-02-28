using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO
{
    public class ComponentTypeDto : ComponentsContainerDTO
    {
        public string BaseComponentType;

        public ComponentTypeDto(IComponentType component) : base(component)
        {
            BaseComponentType = component.BaseComponentTypeId;
        }
    }
}