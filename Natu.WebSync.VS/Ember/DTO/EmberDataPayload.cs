using System.Collections.Generic;

namespace RoslynSpike.Ember.DTO {
    public class EmberDataPayload
    {
        public List<ServiceDto> services;
        public List<PageTypeDto> pageTypes;
        public List<ComponentTypeDto> componentTypes;
        public List<ComponentDto> components;
    }
}