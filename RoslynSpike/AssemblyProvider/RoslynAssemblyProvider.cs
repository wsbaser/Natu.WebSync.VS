using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;

namespace RoslynSpike.Compiler {
    public class RoslynAssemblyProvider : IAssemblyProvider {
        private readonly Workspace _workspace;

        public RoslynAssemblyProvider(Workspace workspace) {
            _workspace = workspace;
        }

        public Tuple<Assembly, Assembly> GetAssemblies() {
            var success = true;
            ProjectDependencyGraph projectGraph = _workspace.CurrentSolution.GetProjectDependencyGraph();
            List<Assembly> assemblies = new List<Assembly>();

            foreach (ProjectId projectId in projectGraph.GetTopologicallySortedProjects()) {
                Compilation projectCompilation = _workspace.CurrentSolution.GetProject(projectId).GetCompilationAsync().Result;
                if (null != projectCompilation && !string.IsNullOrEmpty(projectCompilation.AssemblyName)) {
                    using (var ms = new MemoryStream()) {
                        EmitResult result = projectCompilation.Emit(ms);
                        if (result.Success) {
                            ms.Seek(0, SeekOrigin.Begin);
                            Assembly assembly = Assembly.Load(ms.ToArray());
                            assemblies.Add(assembly);
                        }
                        else {
                            success = false;
                        }
                    }
                }
                else {
                    success = false;
                }
            }
            // SPIKE: Get tests assembly if exist
            if (success) {
                var natuAssembly = assemblies.SingleOrDefault(a => a.FullName.Contains("automateit"));
                var testsAssembly = assemblies.SingleOrDefault(a => a.FullName.Contains("km.tests.selenium"));
                if (natuAssembly == null || testsAssembly == null) {
                    return null;
                }
                return new Tuple<Assembly, Assembly>(natuAssembly,testsAssembly);
            }
            return null;
        }

    }
}