using System.Collections.Generic;
using System.Linq;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynSessionWeb : ISessionWeb {
        public RoslynSessionWeb(IEnumerable<RoslynService> services, IEnumerable<RoslynComponent> components,
            IEnumerable<RoslynPage> pages) {
            Services = services.ToList<IService>();
            Components = components.ToList<IComponent>();
            Pages = pages.ToList<IPage>();
        }

        public List<IPage> Pages { get; }
        public List<IService> Services { get; }
        public List<IComponent> Components { get; }
    }
}