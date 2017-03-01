using Microsoft.CodeAnalysis;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public abstract class RoslynNamedTypeWrapper<T> : RoslynModelWithId<T> {
        protected readonly INamedTypeSymbol Type;
        public string TypeName { get; private set; }

        protected RoslynNamedTypeWrapper(INamedTypeSymbol type) {
            Type = type;
        }

        public override void Fill() {
            Id = Type.ToString();
            TypeName = Type.Name;
        }
    }
}