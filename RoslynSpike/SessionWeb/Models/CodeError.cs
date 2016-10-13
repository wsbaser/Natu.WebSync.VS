namespace RoslynSpike.SessionWeb.Models {
    public class CodeError:ICodeError
    {
        public CodeError(string message, CodeErrorType type)
        {
            Message = message;
            Type = type;
        }

        public string Message { get; }
        public CodeErrorType Type { get; }
    }
}