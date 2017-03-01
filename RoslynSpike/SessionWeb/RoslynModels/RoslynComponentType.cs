using System;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynComponentType : RoslynComponentsContainer<IComponentType>, IComponentType {
        public string BaseComponentTypeId { get; private set; }

        public RoslynComponentType(INamedTypeSymbol componentType) : base(componentType) {
        }

        public override void Fill() {
            base.Fill();
            BaseComponentTypeId = Type.Name == ReflectionNames.BASE_COMPONENT_TYPE ? null : Type.BaseType.ToString();
        }

        public override void SynchronizeTo(IComponentType model) {
            throw new NotImplementedException();
        }
    }
}