using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynComponentInstance : RoslynComponentInstanceBase<IComponentInstance>, IComponentInstance {
        public RoslynComponentInstance(string parentId, IFieldSymbol field, AttributeData attr)
            : base(parentId, field, attr) {
        }

        public override void Fill() {
            base.Fill();
        }

        protected override string GetTypeName() {
            return Field.Type.GetFullTypeName();
        }

        protected override string GetRootScss() {
            var constructorArguments = Attr.GetAttributeConstructorArguments();
            return constructorArguments.Count > 0 ? constructorArguments.First() : null;
        }

        public override void SynchronizeTo(IComponentInstance model)
        {
            throw new NotImplementedException();
        }
    }
}