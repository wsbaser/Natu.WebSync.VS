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
            return Attr.GetAttributeNamedArgument(ReflectionNames.WCA_WEA_NAME_PARAM);
        }

        protected virtual string GetRootScss() {
            return Attr.GetAttributeNamedArgument(ReflectionNames.WEA_SCSS_PARAM);
        }

        protected string GenerateId(string parentId, string fieldName) {
            return $"{parentId}.{fieldName}";
        }
    }
}