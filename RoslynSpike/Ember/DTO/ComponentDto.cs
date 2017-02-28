using RoslynSpike.SessionWeb.Models;
using System.Collections.Generic;

namespace RoslynSpike.Ember.DTO
{
    public class ComponentDto : EmberDtoBase
    {
        public string componentType { get; }
        public string name { get; }
        public string rootScss { get; }
        public IEnumerable<string> constructorParams { get; }

        public ComponentDto(IComponentInstance component) : base(component.Id)
        {
            componentType = component.ComponentType;
            name = component.Name;
            rootScss = component.RootScss;
            constructorParams = component.ConstructorParams;
        }
    }
}