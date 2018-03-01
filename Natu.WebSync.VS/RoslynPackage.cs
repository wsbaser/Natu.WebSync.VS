using System;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Composition;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using RoslynSpike;
using RoslynSpike.BrowserConnection;
using RoslynSpike.BrowserConnection.WebSocket;
using RoslynSpike.Compiler;
using RoslynSpike.Converter;
using RoslynSpike.Ember;
using RoslynSpike.SessionWeb;
using Task = System.Threading.Tasks.Task;

namespace Natu.WebSync.VS
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(RoslynPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    public sealed class RoslynPackage : Package
    {
        /// <summary>
        /// RoslynPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "cddc7d5a-732e-48ab-9fe1-0df68192e1f2";

        /// <summary>
        /// Initializes a new instance of the <see cref="RoslynPackage"/> class.
        /// </summary>
        public RoslynPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        private SynchronizeIt _synchronizeIt;
        private SelectorsConverter _scssConverter;

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            var componentModel = GetComponentModel();
            if (componentModel == null)
                return;

            var workspace = componentModel.GetService<VisualStudioWorkspace>();
            var browserConnection = CreateBrowserConnection();
            var seleniumContextProvider = CreateSeleniumContextProvider(workspace);
            var assemblyProvier = CreateAssemblyProvider(workspace);
            _synchronizeIt = new SynchronizeIt(workspace, browserConnection, seleniumContextProvider, assemblyProvier);
            _scssConverter = new SelectorsConverter(browserConnection);

        }

        private IAssemblyProvider CreateAssemblyProvider(VisualStudioWorkspace workspace) => new RoslynAssemblyProvider(workspace);

        private ISessionWebPovider CreateSeleniumContextProvider(VisualStudioWorkspace workspace) {
            var roslynSessionWebProvider = new RoslynSessionWebProvider(workspace);
            return roslynSessionWebProvider;
        }

        private IBrowserConnection CreateBrowserConnection()
        {
            try
            {
//#if !DEBUG
//                var connection = new WebSocketBrowserConnection(18000, "/websync", new EmberSerializer());
//#else
                var connection = new WebSocketBrowserConnection(18488, "/websync", new EmberSerializer());
//#endif

                connection.Connect();
                return connection;
            }
            catch (Exception)
            {
                Console.WriteLine();
                throw;
            }
        }

        ServiceProvider provider;

        ServiceProvider GetServiceProvider() {
            if (provider == null)
                provider = new ServiceProvider(GetGlobalService(typeof(SDTE)) as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            return provider;
        }

        IComponentModel GetComponentModel() {
            var provider = GetServiceProvider();
            if (provider == null)
                return null;
            return provider.GetService(typeof(SComponentModel)) as IComponentModel;
        }
        #endregion
    }
}
