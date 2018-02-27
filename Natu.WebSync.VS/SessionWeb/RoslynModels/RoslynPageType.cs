using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynPageType : RoslynComponentsContainer<IPageType>, IPageType {
        public string BasePageTypeId { get; private set; }
        public string AbsolutePath { get; private set; }

        public RoslynPageType(INamedTypeSymbol pageType) : base(pageType) {
        }

        public override void Fill()
        {
            base.Fill();
            BasePageTypeId = Type.Name == ReflectionNames.BASE_PAGE_TYPE ? null : Type.BaseType.ToString();
            AbsolutePath = GetAbsolutePath();
        }

        private string GetAbsolutePath()
        {
            return null;
        }

        public override void SynchronizeTo(IPageType model) {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoslynPageType pageType2))
            {
                return false;
            }

            if (Components.Count != pageType2.Components.Count ||
                !Components.SequenceEqual(pageType2.Components, new ComponentInstanceComparer()))
            {
                return false;
            }

            return string.Equals(BasePageTypeId, pageType2.BasePageTypeId) &&
                   string.Equals(AbsolutePath, pageType2.AbsolutePath);
        }
    }
}