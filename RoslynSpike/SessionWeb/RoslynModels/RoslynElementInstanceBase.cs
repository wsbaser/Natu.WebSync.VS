using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public abstract class RoslynElementInstanceBase<T> : RoslynModelWithId<T>, IElementInstance {
        public string FieldName { get; private set; }
        public string Name { get; private set; }
        public string RootScss { get; private set; }
        public string Type { get; private set; }
        public string ParentId { get; }

        protected IFieldSymbol Field;
        protected AttributeData Attr;

        protected RoslynElementInstanceBase(string parentId, IFieldSymbol field, AttributeData attr) {
            ParentId = parentId;
            Field = field;
            Attr = attr;
        }

        public override void Fill() {
            RootScss = GetRootScss();
            Name = GetName();
            Type = GetTypeName();
            FieldName = GetFieldName();
            Id = GenerateId(ParentId, Type, FieldName);
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

        protected string GenerateId(string parentId, string typeName, string fieldName) {
            return $"{parentId}-{typeName}-{fieldName}";
        }
    }
}