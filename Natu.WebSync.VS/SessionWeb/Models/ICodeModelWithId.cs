namespace RoslynSpike.SessionWeb.Models {
    public interface ICodeModelWithId:ICodeModel
    {
        string Id { get; }
    }
}