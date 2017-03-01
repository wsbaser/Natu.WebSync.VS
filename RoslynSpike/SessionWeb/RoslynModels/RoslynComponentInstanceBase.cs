using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public abstract class RoslynComponentInstanceBase<T> : RoslynModelWithId<T>, IComponentInstance {
        public string FieldName { get; private set; }
        public string Name { get; private set; }
        public string RootScss { get; private set; }
        public string ComponentType { get; private set; }
        public string ParentId { get; }
        public IEnumerable<string> ConstructorParams { get; set; }

        protected IFieldSymbol Field;
        protected AttributeData Attr;

        protected RoslynComponentInstanceBase(string parentId, IFieldSymbol field, AttributeData attr) {
            ParentId = parentId;
            Field = field;
            Attr = attr;
        }

        public override void Fill() {
            RootScss = GetRootScss();
            Name = GetName();
            ComponentType = GetTypeName();
            FieldName = GetFieldName();
            Id = GenerateId(ParentId, FieldName);
            ConstructorParams = GetParams();
        }

        public List<string> GetParams()
        {
            var constructorArguments = Attr.GetAttributeConstructorArguments();
            if (constructorArguments.Count > 0)
            {
                constructorArguments.RemoveAt(0);
            }
            return constructorArguments;
        }

        protected virtual string GetFieldName() {
            return Field.Name;
        }

        protected virtual string GetTypeName() {
            return Field.Type.Name;
        }

        protected virtual string GetName() {
            var name = Attr.GetAttributeNamedArgument(ReflectionNames.WCA_WEA_NAME_PARAM);
            if (string.IsNullOrEmpty(name)) {
                name = GetFieldName();
            }
            return name;
        }

        protected virtual string GetRootScss() {
            switch (Attr.AttributeClass.Name) {
                case ReflectionNames.WEB_ELEMENT_ATTRRIBUTE:
                    return Attr.GetAttributeNamedArgument(ReflectionNames.WEA_SCSS_PARAM);
                case ReflectionNames.WEB_COMPONENT_ATTRRIBUTE:
                    var args = Attr.GetAttributeConstructorArguments();
                    return args.Count > 0 ? args[0] : null;
                default:
                    throw new ArgumentOutOfRangeException("Attr.AttributeClass.Name");
            }
        }

        protected string GenerateId(string parentId, string fieldName) {
            return $"{parentId}.{fieldName}";
        }
    }
}