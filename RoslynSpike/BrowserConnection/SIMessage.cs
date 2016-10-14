using System;
using Newtonsoft.Json;
using NLog;

namespace RoslynSpike.BrowserConnection
{
    public class SIMessage
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public SIMessage(SIMessageType type, string data)
        {
            Type = type;
            Data = data;
        }

        public SIMessageType Type { get; }
        public string Data { get; }

        public static SIMessage CreateWebSessionData(string data)
        {
            return new SIMessage(SIMessageType.SessionWebData, data);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(new {Type = Type.ToString(), Data});
        }

        public static SIMessage Deserialize(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<SIMessage>(data);
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw;
            }
        }

        public static SIMessage CreateConvertedSelectorData(string data) {
            return new SIMessage(SIMessageType.ConvertedSelector, data);
        }
    }
}