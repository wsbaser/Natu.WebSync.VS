using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;

namespace RoslynSpike.Compiler
{
    public class RoslynAssemblyProvider:IAssemblyProvider {
        private Workspace _workspace;

        public RoslynAssemblyProvider(Workspace workspace) {
            _workspace = workspace;
        }

        public Assembly GetAssembly() {
            var success = true;

//            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
//            Solution solution = workspace.OpenSolutionAsync(solutionUrl).Result;
            ProjectDependencyGraph projectGraph = _workspace.CurrentSolution.GetProjectDependencyGraph();
            //Dictionary<string, Stream> assemblies = new Dictionary<string, Stream>();
            List<Assembly> assemblies = new List<Assembly>();

            foreach (ProjectId projectId in projectGraph.GetTopologicallySortedProjects())
            {
                Compilation projectCompilation = _workspace.CurrentSolution.GetProject(projectId).GetCompilationAsync().Result;
                if (null != projectCompilation && !string.IsNullOrEmpty(projectCompilation.AssemblyName))
                {
                    using (var ms = new MemoryStream())
                    {
                        EmitResult result = projectCompilation.Emit(ms);
                        if (result.Success)
                        {
                            ms.Seek(0, SeekOrigin.Begin);
                            Assembly assembly = Assembly.Load(ms.ToArray());
                            assemblies.Add(assembly);
//                            string fileName = string.Format("{0}.dll", projectCompilation.AssemblyName);
//
//                            using (FileStream file = File.Create(outputDir + '\\' + fileName))
//                            {
//                                stream.Seek(0, SeekOrigin.Begin);
//                                stream.CopyTo(file);
//                            }
                        }
                        else
                        {
                            success = false;
                        }
                    }
                }
                else
                {
                    success = false;
                }
            }
            if (success) {
                return assemblies.Last();
            }
            else {
                return null;
            }
        }
    }
}
