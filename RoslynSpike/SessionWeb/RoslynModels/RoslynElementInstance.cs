using System;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels
{
    public class RoslynElementInstance :RoslynElementInstanceBase<IElementInstance> {
        public RoslynElementInstance(string parentId, IFieldSymbol field, AttributeData attr) : base(parentId, field, attr) {
        }

        public override void SynchronizeTo(IElementInstance model) {
            throw new NotImplementedException();
        }
    }

}