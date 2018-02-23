using System.Collections.Generic;

namespace RoslynSpike.SessionWeb.Models {
    public interface IComponentInstance : ICodeModelWithId {
        string FieldName { get; }
        string Name { get; }
        Scss.Scss RootSelector { get; }
        string ComponentType { get; }
        IEnumerable<string> ConstructorParams { get; }
    }
}