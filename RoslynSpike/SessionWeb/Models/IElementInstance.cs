namespace RoslynSpike.SessionWeb.Models {
    public interface IElementInstance : ICodeModelWithId {
        string FieldName { get; }
        string Name { get; }
        string RootScss { get; }
        string Type { get; }
    }
}