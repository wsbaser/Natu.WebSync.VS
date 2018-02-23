using System;

namespace RoslynSpike.Compiler {
    public interface IAssemblyProvider {
        Tuple<CompiledProjectAssembly, CompiledProjectAssembly> GetAssemblies();
    }
}