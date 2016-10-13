using System.Collections.Generic;

namespace RoslynSpike.SessionWeb.Models {
    public interface ISessionWeb {
        List<IPage> Pages { get; }
        List<IService> Services { get; }
        List<IComponent> Components { get; }
    }
}