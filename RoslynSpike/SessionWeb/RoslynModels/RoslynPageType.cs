using System;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynPageType : RoslynComponentsContainer<IPageType>, IPageType {
        public string BasePageTypeId { get; private set; }
        public string AbsolutePath { get; private set; }

        public RoslynPageType(INamedTypeSymbol pageType) : base(pageType) {
        }

        public override void Fill()
        {
            base.Fill();
            BasePageTypeId = Type.BaseType.GetFullTypeName();
            AbsolutePath = GetAbsolutePath();
        }

        private string GetAbsolutePath()
        {
            return null;
        }

        public override void SynchronizeTo(IPageType model) {
            throw new NotImplementedException();
        }
    }
}