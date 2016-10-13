using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynComponent : RoslynElementsContainer<IComponent>, IComponent {
        public RoslynComponent(INamedTypeSymbol type) : base(type) {
            Elements = new List<IElementInstance>();
        }

        public override void SynchronizeTo(IComponent model) {
            throw new NotImplementedException();
        }
    }
}