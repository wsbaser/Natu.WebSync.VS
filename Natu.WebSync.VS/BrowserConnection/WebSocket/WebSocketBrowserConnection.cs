using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Moq;
using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using RoslynSpike.Converter;
using RoslynSpike.Reflection;
using RoslynSpike.SessionWeb.Models;
using WebSocketSharp.Server;

namespace RoslynSpike.BrowserConnection.WebSocket
{
    public class WebSocketBrowserConnection : IBrowserConnection
    {
        private static NLog.Logger _log = LogManager.GetCurrentClassLogger();

        private WebSocketServer _webSocketServer;
        private readonly int _serverPort;
        private readonly string _path;

        public event EventHandler SessionWebRequested;
        public event EventHandler<IEnumerable<ISessionWeb>> SessionWebReceived;
        public event EventHandler<string> UrlToMatchReceived;
        public event EventHandler<string> SelectorToConvertReceived;
        public event EventHandler<SIMessage> Broadcasted;

        public ISessionWebSerializer Serializer { get; }
        

        public bool Connected => Broadcasted != null && Broadcasted.GetInvocationList().Length > 0;

        public WebSocketBrowserConnection(int serverPort, string path, ISessionWebSerializer serializer)
        {
            _serverPort = serverPort;
            _path = path;
            Serializer = serializer;
        }

        public void Connect() {
            if (_webSocketServer == null) {
                _webSocketServer = CreateSocketServer(_serverPort, _path);
            }
            try {
                _webSocketServer.Start();
            }
            catch (SocketException e) {
                _log.Error($@"Unable to start server {_webSocketServer.Address}", e);
            }
        }

        private WebSocketServer CreateSocketServer(int port, string path)
        {
            var webSocket = new WebSocketServer(port);
            webSocket.AddWebSocketService(path, () => new SynchronizeBehaviour(this));
            return webSocket;
        }

        public void OnMessage(SIMessage message)
        {
            switch (message.Type)
            {
                case SIMessageType.SessionWebData:
                    var sessionWebs = Serializer.Deserialize(message.Data);
                    OnSessionWebReceived(sessionWebs);
                    break;
                case SIMessageType.SessionWebRequest:
                    OnSessionWebRequested();
                    break;
                case SIMessageType.ConvertSelector:
                    OnSelectorToConvertReceived(message.Data);
                    break;
                case SIMessageType.MatchUrl:
                    OnMatchUrlReceived(message.Data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnSessionWebRequested() => SessionWebRequested?.Invoke(this, EventArgs.Empty);

        private void OnMatchUrlReceived(string url) => UrlToMatchReceived?.Invoke(this, url);

        private void OnSelectorToConvertReceived(string selector) => SelectorToConvertReceived?.Invoke(this, selector);

        private void OnSessionWebReceived(IEnumerable<ISessionWeb> sessionWebs) => SessionWebReceived?.Invoke(this, sessionWebs);

        public void SendSelector(Selector selector) => OnBroadcasted(SIMessage.CreateConvertedSelectorData(JsonConvert.SerializeObject(selector)));

        public void SendSessionWeb(IEnumerable<ISessionWeb> webs) => OnBroadcasted(SIMessage.CreateWebSessionData(Serializer.Serialize(webs)));

        public void SendUrlMatchResult(MatchUrlResult matchUrlResult) => OnBroadcasted(SIMessage.CreateUrlMatchResultData(JsonConvert.SerializeObject(matchUrlResult)));

        public void Close()
        {
            _webSocketServer.Stop();
        }

        protected virtual void OnBroadcasted(SIMessage msg)
        {
            Broadcasted?.Invoke(this, msg);
        }
    }

    [TestFixture]
    public class WebSocketBrowserConnectionTest
    {
        private WebSocketBrowserConnection _connection;

        [SetUp]
        public void SetUp()
        {
            var serializerMoq = new Mock<ISessionWebSerializer>();
            _connection = new WebSocketBrowserConnection(18488, "synchronize", serializerMoq.Object);
        }

        [Test]
        public void Connect()
        {
            // .Arrange
            // .Act
            _connection.Connect();
            // .Assert
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
        }
    }
}