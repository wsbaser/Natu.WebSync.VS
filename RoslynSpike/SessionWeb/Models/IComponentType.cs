using System.Collections.Generic;

namespace RoslynSpike.SessionWeb.Models
{
    public interface IComponentsContainer: ICodeModelWithId {
        List<IComponentInstance> Components { get; }
    }

    public interface IComponentType : IComponentsContainer
    {
        string BaseComponentTypeId { get; }
    }
}