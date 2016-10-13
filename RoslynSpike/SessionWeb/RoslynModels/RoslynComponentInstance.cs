using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynComponentInstance : RoslynElementInstance, IComponentInstance {
        public RoslynComponentInstance(string parentId, IFieldSymbol field, AttributeData attr)
            : base(parentId, field, attr) {
        }

        public IEnumerable<string> ConstructorParams { get; set; }

        public override void Fill() {
            base.Fill();
            ConstructorParams = GetParams();
        }

        protected override string GetTypeName() {
            return Field.Type.GetFullTypeName();
        }

        protected override string GetRootScss() {
            var constructorArguments = Attr.GetAttributeConstructorArguments();
            return constructorArguments.Count > 0 ? constructorArguments.First() : null;
        }

        public List<string> GetParams() {
            var constructorArguments = Attr.GetAttributeConstructorArguments();
            if (constructorArguments.Count > 0) {
                constructorArguments.RemoveAt(0);
            }
            return constructorArguments;
        }

        public void SynchronizeTo(IComponentInstance model) {
            throw new NotImplementedException();
        }
    }
}