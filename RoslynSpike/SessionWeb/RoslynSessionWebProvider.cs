using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using NLog;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.SessionWeb.RoslynModels;
using Project = Microsoft.CodeAnalysis.Project;
using Solution = Microsoft.CodeAnalysis.Solution;
using Microsoft.VisualStudio.LanguageServices;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb
{
    public class RoslynSessionWebProvider:ISessionWebPovider
    {
        private VisualStudioWorkspace _workspace;
        private Dictionary<string, ISessionWeb> _cachedSessionWebs;
        INamedTypeSymbol _basePageType;
        INamedTypeSymbol _baseComponentType;
        INamedTypeSymbol _baseServiceType;

        public RoslynSessionWebProvider(VisualStudioWorkspace workspace)
        {
            _workspace = workspace;
        }

        public async Task InitializeAsync()
        {
            _basePageType = await GetTypeByName(_workspace.CurrentSolution, ReflectionNames.BASE_PAGE_TYPE);
            _baseComponentType = await GetTypeByName(_workspace.CurrentSolution, ReflectionNames.BASE_COMPONENT_TYPE);
            _baseServiceType = await GetTypeByName(_workspace.CurrentSolution, ReflectionNames.BASE_SERVICE_TYPE);
        }

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        public async Task<IEnumerable<ISessionWeb>> GetSessionWebsAsync(DocumentId changedDocumentId)
        {
            try
            {
                if (_cachedSessionWebs == null)
                {
                    var document = _workspace.CurrentSolution.GetDocument(changedDocumentId);
                    var syntaxTree = await document.GetSyntaxTreeAsync();
                    var allTypes = syntaxTree.GetRoot().DescendantNodes().OfType<INamedTypeSymbol>();
                    var pages = GetPages(allTypes);
                    var components = GetComppnents(allTypes);
                    return UpdateCachedSessionWebs(pages, components);
                }
                else
                {
                    var solution = _workspace.CurrentSolution;
                    //solution.GetDocument();
                    var services = await GetServicesAsync(solution);
                    var pages = await GetPagesAsync(solution);
                    var components = await GetComponentsAsync(solution);

                    // . for now, we unable to extract sessions, so everything is store in one session
                    var sessionWeb = new RoslynSessionWeb(services, components, pages);
                    var sessionWebs = new List<ISessionWeb> { sessionWeb };
                    CacheSesionWebs(sessionWebs);
                    return sessionWebs;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex,"Unable to collect selenium contexts");
                throw;
            }
        }

        private void CacheSesionWebs(List<ISessionWeb> sessionWebs)
        {
            throw new NotImplementedException();
        }

        private List<ISessionWeb> UpdateCachedSessionWebs(IEnumerable<RoslynPageType> pageTypes, IEnumerable<RoslynComponentType> componentTypes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RoslynComponentType> GetComppnents(IEnumerable<INamedTypeSymbol> types)
        {
            var baseComponentTypeName = _baseComponentType.GetFullTypeName();
            var pageTypes = types.Where(t => t.AllInterfaces.Any(i => i.GetFullTypeName() == baseComponentTypeName));
            return pageTypes.Select(dc =>
            {
                var page = new RoslynComponentType(dc);
                page.Fill();
                return page;
            });
        }

        public IEnumerable<RoslynPageType> GetPages(IEnumerable<INamedTypeSymbol> types)
        {
            var basePageTypeName = _basePageType.GetFullTypeName();
            var pageTypes = types.Where(t => t.AllInterfaces.Any(i => i.GetFullTypeName() == basePageTypeName));
            return pageTypes.Select(dc =>
            {
                var page = new RoslynPageType(dc);
                page.Fill();
                return page;
            });
        }

        private async Task<IEnumerable<RoslynComponentType>> GetComponentsAsync(Solution solution)
        {
            var derivedClasses = await GetDerivedClassesAsync(solution, ReflectionNames.BASE_COMPONENT_TYPE);
            return derivedClasses.Select(dc => {
                var component = new RoslynComponentType(dc);
                component.Fill();
                return component;
            });
        }

        private async Task<IEnumerable<RoslynPageType>> GetPagesAsync(Solution solution)
        {
            var derivedClasses = await GetDerivedClassesAsync(solution, ReflectionNames.BASE_PAGE_TYPE);
            return derivedClasses.Select(dc => {
                var page = new RoslynPageType(dc);
                page.Fill();
                return page;
            });
        }

        private async Task<IEnumerable<RoslynService>> GetServicesAsync(Solution solution)
        {
            var derivedClasses = await GetDerivedClassesAsync(solution, ReflectionNames.BASE_SERVICE_TYPE);
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