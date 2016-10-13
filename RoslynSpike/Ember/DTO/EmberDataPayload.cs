using System.Collections.Generic;

namespace RoslynSpike.Ember.DTO {
    public class EmberDataPayload
    {
        public List<ServiceDto> services;
        public List<PageDto> pages;
        public List<ComponentDto> components;
        public List<ComponentInstanceDto> componentInstances;
        public List<ElementInstanceDto> elementInstances;
    }
}