using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using NLog;
using RoslynSpike.BrowserConnection;
using RoslynSpike.Compiler;
using RoslynSpike.SessionWeb;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike
{
    public class SynchronizeIt
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();
        private readonly Workspace _workspace;
        private readonly IBrowserConnection _browserConnection;
        private readonly ISessionWebPovider _sessionWebProvider;
        private readonly IAssemblyProvider _assemblyProvider;

        private ISessionWeb _sessionWeb;
        

        public SynchronizeIt(Workspace workspace, IBrowserConnection browserConnection,
            ISessionWebPovider sessionWebProvider, IAssemblyProvider assemblyProvider)
        {
            _workspace = workspace;
            _browserConnection = browserConnection;
            _sessionWebProvider = sessionWebProvider;
            _assemblyProvider = assemblyProvider;
            _browserConnection.SessionWebRequested += _browserConnection_SessionWebRequested;
            _browserConnection.SessionWebReceived += BrowserConnectionSessionWebReceived;
            _browserConnection.UrlToMatchReceived += _browserConnection_UrlToMatchReceived;
            _workspace.WorkspaceChanged += _workspace_WorkspaceChanged;
        }

        private void _browserConnection_UrlToMatchReceived(object sender, string url) {
            var assembly = _assemblyProvider.GetAssembly();
        }

        private void _browserConnection_SessionWebRequested(object sender, EventArgs e)
        {
            CollectAndSynchronizeChanges();
        }

        private void BrowserConnectionSessionWebReceived(object sender, IEnumerable<ISessionWeb> args)
        {
            throw new NotImplementedException();
        }

        private void _workspace_WorkspaceChanged(object sender, WorkspaceChangeEventArgs e) {
            if (e.Kind == WorkspaceChangeKind.SolutionAdded) {
                var assembly = _assemblyProvider.GetAssembly();
            }
            if (!_browserConnection.Connected) {
                return;
            }
            // TODO: how to handle other events
            if (e.Kind == WorkspaceChangeKind.DocumentChanged) {
                CollectAndSynchronizeChanges(e.DocumentId);
            }
        }

        private async void CollectAndSynchronizeChanges(DocumentId documentId)
        {
            IEnumerable<ISessionWeb> sessionWebs = await _sessionWebProvider.GetSessionWebsAsync(documentId);
            SynchronizeSessionWebs(sessionWebs);
        }

        private async void CollectAndSynchronizeChanges()
        {
            IEnumerable<ISessionWeb> sessionWebs = await _sessionWebProvider.GetSessionWebsAsync(false);
            SynchronizeSessionWebs(sessionWebs);
        }

        private void SynchronizeSessionWebs(IEnumerable<ISessionWeb> sessionWebs) {
            // . Currently, there is only one
            var firstSessionWeb = sessionWebs.First();
            if (!firstSessionWeb.Equals(_sessionWeb))
            {
                _browserConnection.SendSessionWeb(new[] { firstSessionWeb });
                _sessionWeb = firstSessionWeb;
            }
        }
    }
}
