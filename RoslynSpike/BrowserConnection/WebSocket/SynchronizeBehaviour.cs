using NLog;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    public class SynchronizeBehaviour : WebSocketBehavior
    {
        private static readonly NLog.Logger _log = LogManager.GetCurrentClassLogger();
        private readonly WebSocketBrowserConnection _browserConnection;

        public SynchronizeBehaviour(WebSocketBrowserConnection browserConnection)
        {
            _browserConnection = browserConnection;
            _browserConnection.Broadcasted += _browserConnection_Broadcasted;
        }

        private void _browserConnection_Broadcasted(object sender, SIMessage e)
        {
            Sessions.Broadcast(e.Serialize());
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            _browserConnection.OnMessage(SIMessage.Deserialize(e.Data));
        }
    }
}