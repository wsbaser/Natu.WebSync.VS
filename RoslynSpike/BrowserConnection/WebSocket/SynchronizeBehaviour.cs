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
        }

        protected override void OnOpen() {
            base.OnOpen();
            _browserConnection.Broadcasted += _browserConnection_Broadcasted;
        }

        protected override void OnClose(CloseEventArgs e) {
            base.OnClose(e);
            _browserConnection.Broadcasted -= _browserConnection_Broadcasted;
        }

        protected override void OnError(ErrorEventArgs e) {
            base.OnError(e);
            _log.Error(e.Exception, e.Message);
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