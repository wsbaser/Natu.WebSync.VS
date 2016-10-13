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
            payload.services = web.Services.Select(s => new ServiceDto(s)).ToList();
            payload.components = web.Components.Select(c => new ComponentDto(c)).ToList();
            payload.pages = web.Pages.Select(p => new PageDto(p)).ToList();
            SerializeElementInstances(payload, web);
        }

        private void SerializeElementInstances(EmberDataPayload payload, ISessionWeb web) {
            payload.componentInstances = new List<ComponentInstanceDto>();
            payload.elementInstances = new List<ElementInstanceDto>();

            SerializeElementInstances(payload, web.Pages);
            SerializeElementInstances(payload, web.Components);
        }

        private static void SerializeElementInstances(EmberDataPayload payload,
            IEnumerable<IElementsContainer> container) {
            foreach (var page in container) {
                foreach (var elementInstance in page.Elements) {
                    string parentPageId = null, parentComponentId = null;
                    if (page is IPage) {
                        parentPageId = page.Id;
                    }
                    else if (page is IComponent) {
                        parentComponentId = page.Id;
                    }

                    var componentInstane = elementInstance as IComponentInstance;
                    if (componentInstane != null) {
                        payload.componentInstances.Add(new ComponentInstanceDto(componentInstane, parentPageId,
                            parentComponentId));
                    }
                    else {
                        payload.elementInstances.Add(new ElementInstanceDto(elementInstance, parentPageId,
                            parentComponentId));
                    }
                }
            }
        }
    }
}