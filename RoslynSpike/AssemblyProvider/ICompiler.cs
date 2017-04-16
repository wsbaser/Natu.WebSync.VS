using System;
using System.Reflection;

namespace RoslynSpike.Compiler {
    public interface IAssemblyProvider {
        Tuple<Assembly, Assembly> GetAssemblies();
    }
}