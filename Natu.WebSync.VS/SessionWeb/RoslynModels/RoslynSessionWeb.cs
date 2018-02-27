using System.Collections.Generic;
using System.Linq;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels
{
    public class RoslynSessionWeb : ISessionWeb
    {
        public RoslynSessionWeb(IEnumerable<RoslynService> services, IEnumerable<RoslynComponentType> componentTypes,
            IEnumerable<RoslynPageType> pageTypes)
        {
            Services = services.ToDictionary(s => s.Id, s => (IService) s);
            ComponentTypes = componentTypes.ToDictionary(ct => ct.Id, ct => (IComponentType) ct);
            PageTypes = pageTypes.ToDictionary(pt => pt.Id, pt => (IPageType) pt);
        }

        public Dictionary<string, IPageType> PageTypes { get; }
        public Dictionary<string, IService> Services { get; }
        public Dictionary<string, IComponentType> ComponentTypes { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is RoslynSessionWeb sessionWeb2))
            {
                return false;
            }

            if (Services.Keys.Count != sessionWeb2.Services.Keys.Count ||
                !Services.Keys.All(sessionWeb2.Services.Keys.Contains))
            {
                return false;
            }

            if (PageTypes.Keys.Count != sessionWeb2.PageTypes.Keys.Count ||
                !PageTypes.Keys.All(sessionWeb2.PageTypes.Keys.Contains))
            {
                return false;
            }

            if (ComponentTypes.Keys.Count != sessionWeb2.ComponentTypes.Keys.Count ||
                !ComponentTypes.Keys.All(sessionWeb2.ComponentTypes.Keys.Contains))
            {
                return false;
            }

            foreach (var key in Services.Keys)
            {
                if (!Services[key].Equals(sessionWeb2.Services[key]))
                {
                    return false;
                }
            }

            foreach (var key in PageTypes.Keys)
            {
                if (!PageTypes[key].Equals(sessionWeb2.PageTypes[key]))
                {
                    return false;
                }
            }

            foreach (var key in ComponentTypes.Keys)
            {
                if (!ComponentTypes[key].Equals(sessionWeb2.ComponentTypes[key]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}