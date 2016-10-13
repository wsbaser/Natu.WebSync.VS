using System;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynPage : RoslynElementsContainer<IPage>, IPage {
        public RoslynPage(INamedTypeSymbol pageType) : base(pageType) {
        }

        public override void SynchronizeTo(IPage model) {
            throw new NotImplementedException();
        }
    }
}