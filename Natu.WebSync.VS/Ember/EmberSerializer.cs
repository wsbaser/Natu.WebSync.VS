using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RoslynSpike.BrowserConnection;
using RoslynSpike.Ember.DTO;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember {
    internal class EmberSerializer : ISessionWebSerializer {
        public string Serialize(IEnumerable<ISessionWeb> webs) {
            var payload = new EmberDataPayload();
            foreach (var web in webs) {
                SerializeSessionWeb(payload, web);
            }
            return JsonConvert.SerializeObject(payload);
        }

        public IEnumerable<ISessionWeb> Deserialize(string data) {
            return JsonConvert.DeserializeObject<IEnumerable<ISessionWeb>>(data);
        }

        private void SerializeSessionWeb(EmberDataPayload payload, ISessionWeb web) {
            payload.services = web.Services.Values.Select(s => new ServiceDto(s)).ToList();
            payload.pageTypes = web.PageTypes.Values.Select(p => new PageTypeDto(p)).ToList();
            payload.componentTypes = web.ComponentTypes.Values.Select(c => new ComponentTypeDto(c)).ToList();
            SerializeComponents(payload, web);
        }

        private void SerializeComponents(EmberDataPayload payload, ISessionWeb web)
        {
            payload.components = new List<ComponentDto>();
            SerializeComponents(payload, web.PageTypes.Values);
            SerializeComponents(payload, web.ComponentTypes.Values);
        }

        private static void SerializeComponents(EmberDataPayload payload, IEnumerable<IComponentsContainer> containers)
        {
            foreach (var container in containers)
            {
                foreach (var component in container.Components)
                {
                    payload.components.Add(new ComponentDto(component));
                }
            }
        }
    }
}