namespace RoslynSpike.SessionWeb.Models {
    public interface ICodeError
    {
        string Message { get; }
        CodeErrorType Type { get; }
    }
}