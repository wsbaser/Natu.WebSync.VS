using System.Collections.Generic;
using System.Linq;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynSessionWeb : ISessionWeb {
        public RoslynSessionWeb(IEnumerable<RoslynService> services, IEnumerable<RoslynComponentType> componentTypes,
            IEnumerable<RoslynPageType> pageTypes) {
            Services = services.ToDictionary(s => s.Id, s => (IService) s);
            ComponentTypes = componentTypes.ToDictionary(ct => ct.Id, ct => (IComponentType) ct);
            PageTypes = pageTypes.ToDictionary(pt => pt.Id, pt => (IPageType) pt);
        }

        public Dictionary<string, IPageType> PageTypes { get; }
        public Dictionary<string, IService> Services { get; }
        public Dictionary<string, IComponentType> ComponentTypes { get; }
    }
}