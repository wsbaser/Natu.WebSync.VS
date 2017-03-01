using System.Collections.Generic;
using System.Linq;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO
{
    public class ComponentsContainerDTO:EmberDtoBase {
        public List<string> components;

        public ComponentsContainerDTO(IComponentsContainer container) : base(container.Id) {
            components = container.Components.Select(c => c.Id).ToList();
        }
    }
}