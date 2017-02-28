using System.Collections.Generic;
using System.Linq;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynSessionWeb : ISessionWeb {
        public RoslynSessionWeb(IEnumerable<RoslynService> services, IEnumerable<RoslynComponentType> componentTypes,
            IEnumerable<RoslynPageType> pageTypes) {
            Services = services.ToList<IService>();
            ComponentTypes = componentTypes.ToList<IComponentType>();
            PageTypes = pageTypes.ToList<IPageType>();
        }

        public List<IPageType> PageTypes { get; }
        public List<IService> Services { get; }
        public List<IComponentType> ComponentTypes { get; }
    }
}