using System.Collections.Generic;

namespace RoslynSpike.SessionWeb.Models {
    public interface ISessionWeb {
        Dictionary<string,IPageType> PageTypes { get; }
        Dictionary<string,IService> Services { get; }
        Dictionary<string,IComponentType> ComponentTypes { get; }
    }
}