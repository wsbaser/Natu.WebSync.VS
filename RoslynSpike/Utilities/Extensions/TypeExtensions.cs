using Microsoft.CodeAnalysis;

namespace RoslynSpike.Utilities.Extensions {
    public static class TypeExtensions {
        public static string GetFullTypeName(this ITypeSymbol type) {
            return type.ContainingNamespace.GetFullMetadataName() + "." + type.Name;
        }
    }
}