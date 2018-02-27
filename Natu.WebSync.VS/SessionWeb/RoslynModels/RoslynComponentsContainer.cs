using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public abstract class RoslynComponentsContainer<T> : RoslynNamedTypeWrapper<T>,IComponentsContainer {
        public List<IComponentInstance> Components { get; set; }

        protected RoslynComponentsContainer(INamedTypeSymbol type) : base(type)
        {
        }

        public override void Fill() {
            base.Fill();

            Components = new List<IComponentInstance>();

            var fields = Type.GetMembers().Where(m => m.Kind == SymbolKind.Field || m.Kind == SymbolKind.Property);
            foreach (var symbol in fields)
            {
                var attrs = symbol.GetAttributes();
                var webElementAttr = GetWebElementAttribute(attrs);
                var webComponentAttr = GetWebComponentAttribute(attrs);
                if ((webElementAttr == null && webComponentAttr == null) ||
                    (webElementAttr != null && webComponentAttr != null))
                {
                    // . no element/comonent attributes OR ambigious attributes
                    continue;
                }
                if (webElementAttr != null)
                {
                    // . this is a WebElement field
                    var componentInstance = GetComponentInstance(symbol, webElementAttr);
                    if (componentInstance != null)
                        Components.Add(componentInstance);
                }
                if (webComponentAttr != null)
                {
                    // . this is a WebComponent field
                    var componentInstance = GetComponentInstance(symbol, webComponentAttr);
                    if (componentInstance != null)
                        Components.Add(componentInstance);
                }
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="attrs"></param>
        /// <param name="attributeTypeName"></param>
        /// <returns>
        ///     Returns null if collection has several attributes of the same type
        /// </returns>
        private static AttributeData GetAttributeOfType(ImmutableArray<AttributeData> attrs, string attributeTypeName) {
            var weAttrs = attrs.Where(a => a.AttributeClass.Name == attributeTypeName).ToList();
            return weAttrs.Count == 1 ? weAttrs.First() : null;
        }

        protected RoslynComponentInstance GetComponentInstance(ISymbol symbol, AttributeData webComponentAttr)
        {
            var component = new RoslynComponentInstance(TypeName, symbol, webComponentAttr);
            component.Fill();
            return component;
        }

        protected AttributeData GetWebComponentAttribute(ImmutableArray<AttributeData> attrs)
        {
            return GetAttributeOfType(attrs, ReflectionNames.WEB_COMPONENT_ATTRRIBUTE);
        }

        protected AttributeData GetWebElementAttribute(ImmutableArray<AttributeData> attrs)
        {
            return GetAttributeOfType(attrs, ReflectionNames.WEB_ELEMENT_ATTRRIBUTE);
        }
    }
}