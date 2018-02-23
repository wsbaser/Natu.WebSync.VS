using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace RoslynSpike.Utilities.Extensions
{
    public static class AttributeExtensions
    {
        public static string GetAttributeNamedArgument(this AttributeData attribute, string argumentName)
        {
            var filteredList = attribute.NamedArguments.Where(na => na.Key == argumentName).ToList();
            return filteredList.Count == 0 ? null : filteredList.First().Value.Value.ToString();
        }

        public static List<string> GetAttributeConstructorArguments(this AttributeData attribute)
        {
            return attribute.ConstructorArguments
                .Select(ca => ca.Values.Length == 0 ? null : ca.Values[0].Value.ToString())
                .Where(ca => ca != null).ToList();
        }
    }
}
