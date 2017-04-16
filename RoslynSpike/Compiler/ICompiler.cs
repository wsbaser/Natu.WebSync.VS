using System.Reflection;
using Microsoft.CodeAnalysis;

namespace RoslynSpike.Compiler {
    public interface IAssemblyProvider {
        Assembly GetAssembly();
    }
}