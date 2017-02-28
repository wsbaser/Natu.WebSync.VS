using System.Collections.Generic;

namespace RoslynSpike.SessionWeb.Models {
    public interface ISessionWeb {
        List<IPageType> PageTypes { get; }
        List<IService> Services { get; }
        List<IComponentType> ComponentTypes { get; }
    }
}