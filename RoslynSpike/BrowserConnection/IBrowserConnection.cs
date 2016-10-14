using System;
using System.Collections.Generic;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.BrowserConnection
{
    /// <summary>
    /// Allows to send and receive data to SynchronizeIt browser extension
    /// </summary>
    public interface IBrowserConnection
    {
        ISessionWebSerializer Serializer { get; }
        event EventHandler<string> SelectorReceived;
        void SendSelector(string selector, SelectorType selectorType);
        void SendSessionWeb(IEnumerable<ISessionWeb> webs);
        void Connect();
        void Close();
        event EventHandler<IEnumerable<ISessionWeb>> SessionWebReceived;
        event EventHandler SessionWebRequested;
    }
}