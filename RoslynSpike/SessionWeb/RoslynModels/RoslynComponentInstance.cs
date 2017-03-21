using System;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynComponentInstance : RoslynComponentInstanceBase<IComponentInstance>, IComponentInstance {
        public RoslynComponentInstance(string parentId, ISymbol field, AttributeData attr)
            : base(parentId, field, attr) {
        }

        public override void SynchronizeTo(IComponentInstance model)
        {
            throw new NotImplementedException();
        }
    }
}