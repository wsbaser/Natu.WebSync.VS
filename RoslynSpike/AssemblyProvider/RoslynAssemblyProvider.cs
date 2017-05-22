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
        private Dictionary<string, CompiledProjectAssembly> _compiledAssemblies;

        public RoslynAssemblyProvider(Workspace workspace) {
            _workspace = workspace;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            _compiledAssemblies = new Dictionary<string, CompiledProjectAssembly>();
        }

        public Tuple<CompiledProjectAssembly, CompiledProjectAssembly> GetAssemblies() {
            var success = true;
            ProjectDependencyGraph projectGraph = _workspace.CurrentSolution.GetProjectDependencyGraph();
            if (_compiledAssemblies.Count == 0) {
                _compiledAssemblies.Clear();
                foreach (ProjectId projectId in projectGraph.GetTopologicallySortedProjects()) {
                    var project = _workspace.CurrentSolution.GetProject(projectId);
                    Compilation projectCompilation = project.GetCompilationAsync().Result;
                    if (null != projectCompilation && !string.IsNullOrEmpty(projectCompilation.AssemblyName)) {
                        var dependencies = new Dictionary<string, MetadataReference>();
                        var assemblyNames = projectCompilation.ReferencedAssemblyNames.ToList();
                        var assembleReferences = projectCompilation.References.ToList();
                        for (int i = 0; i < assemblyNames.Count; i++) {
                            dependencies.Add(assemblyNames[i].ToString(), assembleReferences[i]);
                        }
                        using (var ms = new MemoryStream()) {
                            EmitResult result = projectCompilation.Emit(ms);
                            if (result.Success) {
                                ms.Seek(0, SeekOrigin.Begin);
                                Assembly assembly = Assembly.Load(ms.ToArray());
                                var projectPath = new FileInfo(project.FilePath).Directory.FullName;
                                _compiledAssemblies.Add(assembly.FullName, new CompiledProjectAssembly(assembly, projectPath, dependencies));
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
            }
            // SPIKE: Get tests assembly if exist
            if (success) {
                var natuAssembly = _compiledAssemblies.Values.SingleOrDefault(a => a.Assembly.FullName.Contains("automateit"));
                var testsAssembly = _compiledAssemblies.Values.SingleOrDefault(a => a.Assembly.FullName.Contains("km.tests.selenium"));
                if (natuAssembly == null || testsAssembly == null) {
                    return null;
                }
                return new Tuple<CompiledProjectAssembly, CompiledProjectAssembly>(natuAssembly,testsAssembly);
            }
            return null;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (_compiledAssemblies.ContainsKey(args.Name)) {
                return _compiledAssemblies[args.Name].Assembly;
            }
            foreach (var compiledAssembly in _compiledAssemblies.Values) {
                if (compiledAssembly.Dependencies.ContainsKey(args.Name)) {
                    return Assembly.LoadFrom(compiledAssembly.Dependencies[args.Name].Display);
                }
            }
            return null;
        }
    }

    public class CompiledProjectAssembly {
        public Assembly Assembly;
        public string ProjectPath;
        public Dictionary<string, MetadataReference> Dependencies;

        public CompiledProjectAssembly(Assembly assembly, string projectPath, Dictionary<string, MetadataReference> dependencies) {
            Assembly = assembly;
            ProjectPath = projectPath;
            Dependencies = dependencies;
        }
    }
}