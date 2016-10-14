using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.Scss {
    internal enum AttributeMatchStyle {
        [StringValue("=")]
        Equal,
        [StringValue("~")]
        Contains
    }
}