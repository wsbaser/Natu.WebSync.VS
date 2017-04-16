using System;
using System.Collections.Generic;
using RoslynSpike.Converter;
using RoslynSpike.Reflection;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.BrowserConnection
{
    /// <summary>
    /// Allows to send and receive data to SynchronizeIt browser extension
    /// </summary>
    public interface IBrowserConnection
    {
        ISessionWebSerializer Serializer { get; }
        void Connect();
        void Close();
        bool Connected { get; }
        event EventHandler<IEnumerable<ISessionWeb>> SessionWebReceived;
        event EventHandler SessionWebRequested;
        event EventHandler<string> SelectorToConvertReceived;
        event EventHandler<string> UrlToMatchReceived;
        void SendSelector(Selector selector);
        void SendSessionWeb(IEnumerable<ISessionWeb> webs);
        void SendUrlMatchResult(MatchUrlResult matchUrlResult);
    }
}