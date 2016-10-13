using System.Collections.Generic;

namespace RoslynSpike.SessionWeb.Models
{
    public interface IElementsContainer: ICodeModelWithId {
        List<IElementInstance> Elements { get; }
    }

    public interface IComponent : IElementsContainer{
    }
}