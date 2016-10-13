using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public abstract class RoslynElementsContainer<T> : RoslynNamedTypeWrapper<T>,IElementsContainer {
        public List<IElementInstance> Elements { get; set; }

        protected RoslynElementsContainer(INamedTypeSymbol type) : base(type) {
        }

        public override void Fill() {
            base.Fill();

            Elements = new List<IElementInstance>();

            var fields = Type.GetMembers().Where(m => m.Kind == SymbolKind.Field);
            foreach (var symbol in fields)
            {
                var field = (IFieldSymbol)symbol;
                var attrs = field.GetAttributes();
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
                    var elementInstance = GetElementInstance(field, webElementAttr);
                    if (elementInstance != null)
                        Elements.Add(elementInstance);
                }
                if (webComponentAttr != null)
                {
                    // . this is a WebComponent field
                    var componentInstance = GetComponentInstance(field, webComponentAttr);
                    if (componentInstance != null)
                        Elements.Add(componentInstance);
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

        private WebElementType ParseElementType(string typeName) {
            switch (typeName) {
                case "WebText":
                    return WebElementType.WebText;
                case "WebButton":
                    return WebElementType.WebButton;
                case "WebCheckbox":
                    return WebElementType.WebCheckbox;
                case "WebImage":
                    return WebElementType.WebImage;
                case "WebInput":
                    return WebElementType.WebInput;
                case "WebLink":
                    return WebElementType.WebLink;
                case "WebRadioButton":
                    return WebElementType.WebRadioButton;
                default:
                    return WebElementType.Unknown;
            }
        }

        protected RoslynComponentInstance GetComponentInstance(IFieldSymbol field, AttributeData webComponentAttr)
        {
            var component = new RoslynComponentInstance(Id, field, webComponentAttr);
            component.Fill();
            return component;
        }

        protected RoslynElementInstance GetElementInstance(IFieldSymbol field, AttributeData webElementAttr)
        {
            var element = new RoslynElementInstance(Id, field, webElementAttr);
            element.Fill();
            return ParseElementType(element.Type) == WebElementType.Unknown ? null : element;
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