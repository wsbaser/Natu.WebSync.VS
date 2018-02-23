using System.Collections.Generic;

namespace RoslynSpike.SessionWeb.Models {
    public interface ICodeModel {
        List<ICodeError> CodeErrors { get; }
    }
}