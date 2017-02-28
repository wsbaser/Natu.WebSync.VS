using System;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynComponentType : RoslynComponentsContainer<IComponentType>, IComponentType {
        public string BaseComponentTypeId { get; private set; }

        public RoslynComponentType(INamedTypeSymbol componentType) : base(componentType) {
        }

        public override void Fill()
        {
            base.Fill();
            BaseComponentTypeId = Type.BaseType.GetFullTypeName();
        }

        public override void SynchronizeTo(IComponentType model) {
            throw new NotImplementedException();
        }
    }
}