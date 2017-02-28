namespace RoslynSpike.SessionWeb.Models
{
    public interface IPageType : IComponentsContainer
    {
        string BasePageTypeId { get; }
        string AbsolutePath { get; }
    }
}