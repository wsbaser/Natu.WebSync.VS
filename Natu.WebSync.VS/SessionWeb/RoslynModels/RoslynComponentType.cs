using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels
{
    public class RoslynComponentType : RoslynComponentsContainer<IComponentType>, IComponentType
    {
        public string BaseComponentTypeId { get; private set; }

        public RoslynComponentType(INamedTypeSymbol componentType) : base(componentType)
        {
        }

        public override void Fill()
        {
            base.Fill();
            BaseComponentTypeId = Type.Name == ReflectionNames.BASE_COMPONENT_TYPE ? null : Type.BaseType.ToString();
        }

        public override void SynchronizeTo(IComponentType model)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoslynComponentType componentType2))
            {
                return false;
            }

            if (Components.Count != componentType2.Components.Count ||
                !Components.SequenceEqual(componentType2.Components, new ComponentInstanceComparer()))
            {
                return false;
            }

            return string.Equals(BaseComponentTypeId, componentType2.BaseComponentTypeId);
        }
    }
}