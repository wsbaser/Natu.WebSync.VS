using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using NLog;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.SessionWeb.RoslynModels;
using Project = Microsoft.CodeAnalysis.Project;
using Solution = Microsoft.CodeAnalysis.Solution;

namespace RoslynSpike.SessionWeb
{
    public class RoslynSessionWebProvider:ISessionWebPovider
    {
        private const string BASE_SERVICE_TYPE = "ServiceImpl";
        private const string BASE_PAGE_TYPE = "PageBase";
        private const string BASE_COMPONENT_TYPE = "ComponentBase";

        private static Logger _log = LogManager.GetCurrentClassLogger();
        public async Task<IEnumerable<ISessionWeb>> GetSessionWebsAsync(Workspace workspace)
        {
            try
            {
                var solution = workspace.CurrentSolution;

                var services = await GetServicesAsync(solution);
                var pages = await GetPagesAsync(solution);
                var components = await GetComponentsAsync(solution);

                // . for now, we unable to extract sessions, so everything is store in one session
                var sessionWeb = new RoslynSessionWeb(services, components,pages);
                return new List<ISessionWeb> {sessionWeb};
            }
            catch (Exception ex)
            {
                _log.Error(ex,"Unable to collect selenium contexts");
                throw;
            }
        }

        private async Task<IEnumerable<RoslynComponent>> GetComponentsAsync(Solution solution)
        {
            var derivedClasses = await GetDerivedClassesAsync(solution, BASE_COMPONENT_TYPE);
            return derivedClasses.Select(dc => {
                var component = new RoslynComponent(dc);
                component.Fill();
                return component;
            });
        }

        private async Task<IEnumerable<RoslynPage>> GetPagesAsync(Solution solution)
        {
            var derivedClasses = await GetDerivedClassesAsync(solution, BASE_PAGE_TYPE);
            return derivedClasses.Select(dc => {
                var page = new RoslynPage(dc);
                page.Fill();
                return page;
            });
        }

        private async Task<IEnumerable<RoslynService>> GetServicesAsync(Solution solution)
        {
            var derivedClasses = await GetDerivedClassesAsync(solution, BASE_SERVICE_TYPE);
            return derivedClasses.Select(dc => {
                var service = new RoslynService(dc);
                service.Fill();
                return service;
            });
        }

        private async Task<IEnumerable<INamedTypeSymbol>> GetDerivedClassesAsync(Solution solution, string baseClassName)
        {
            INamedTypeSymbol baseType = await GetTypeByName(solution, baseClassName);
            if (baseType != null)
            {
                return (await SymbolFinder
                    .FindDerivedClassesAsync(baseType, solution, solution.Projects.ToImmutableHashSet())
                    .ConfigureAwait(false)).Where(dc => !dc.IsAbstract);
            }
            return new List<INamedTypeSymbol>();
        }

        private async Task<INamedTypeSymbol> GetTypeByName(Solution solution, string className)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    SemanticModel semantic = await document.GetSemanticModelAsync().ConfigureAwait(false);
                    var baseType = semantic.Compilation.GetSymbolsWithName(name => name == className, SymbolFilter.Type).FirstOrDefault() as INamedTypeSymbol;
                    if (baseType != null)
                        return baseType;
                }
            }
            return null;
        }
    }
}