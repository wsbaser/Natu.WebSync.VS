using RoslynSpike.SessionWeb.Models;
using System.Collections.Generic;

namespace RoslynSpike.Ember.DTO
{
    public class ComponentDto : EmberDtoBase
    {
        public string componentType { get; }
        public string name { get; }
        public object rootSelector { get; }
        public IEnumerable<string> constructorParams { get; }

        public ComponentDto(IComponentInstance component) : base(component.Id) {
            componentType = component.ComponentType;
            name = component.Name;
            if (component.RootSelector != null) {
                // TODO(**): save scss value even if we can't parse it
                rootSelector = new
                {
                    combineWithRoot = component.RootSelector.CombineWithRoot,
                    scss = component.RootSelector.Value,
                    css = component.RootSelector.Css,
                    xpath = component.RootSelector.Xpath
                };
            }
            constructorParams = component.ConstructorParams;
        }
    }
}