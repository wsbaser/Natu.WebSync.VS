using System.Collections.Generic;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO {
    public class ComponentInstanceDto : ElementInstanceDto {
        public IEnumerable<string> constructorParams { get; }

        public ComponentInstanceDto(IComponentInstance componentInstance, string parentPageId,
            string parentComponentId) : base(componentInstance, parentPageId, parentComponentId) {
            constructorParams = componentInstance.ConstructorParams;
        }
    }
}