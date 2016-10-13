using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using NLog;
using RoslynSpike.BrowserConnection;
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

        private ISessionWeb _sessionWeb;

        public SynchronizeIt(Workspace workspace, IBrowserConnection browserConnection,
            ISessionWebPovider sessionWebProvider)
        {
            _workspace = workspace;
            _browserConnection = browserConnection;
            _sessionWebProvider = sessionWebProvider;
            _browserConnection.SessionWebRequested += _browserConnection_SessionWebRequested;
            _browserConnection.SessionWebReceived += BrowserConnectionSessionWebReceived;
            _workspace.WorkspaceChanged += _workspace_WorkspaceChanged;
        }

        private void _browserConnection_SessionWebRequested(object sender, EventArgs e)
        {
            CollectAndSynchronizeChanges();
        }

        private void BrowserConnectionSessionWebReceived(object sender, IEnumerable<ISessionWeb> args)
        {
            throw new NotImplementedException();
        }

        private void _workspace_WorkspaceChanged(object sender, WorkspaceChangeEventArgs e)
        {
            CollectAndSynchronizeChanges();
        }

        private async void CollectAndSynchronizeChanges()
        {
            IEnumerable<ISessionWeb> sessionWebs = await _sessionWebProvider.GetSessionWebsAsync(_workspace);
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
