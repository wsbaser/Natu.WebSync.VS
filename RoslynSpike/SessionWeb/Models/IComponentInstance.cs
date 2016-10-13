using System.Collections.Generic;

namespace RoslynSpike.SessionWeb.Models
{
    public interface IComponentInstance : IElementInstance {
        IEnumerable<string> ConstructorParams { get; set; }
    }
}
