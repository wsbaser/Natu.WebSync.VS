using Microsoft.CodeAnalysis;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public abstract class RoslynNamedTypeWrapper<T> : RoslynModelWithId<T> {
        protected readonly INamedTypeSymbol Type;

        protected RoslynNamedTypeWrapper(INamedTypeSymbol type) {
            Type = type;
        }

        public override void Fill() {
            Id = Type.GetFullTypeName();
        }
    }
}