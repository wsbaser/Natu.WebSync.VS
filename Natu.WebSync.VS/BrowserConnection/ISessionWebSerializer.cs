using System.Collections.Generic;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.BrowserConnection {
    public interface ISessionWebSerializer
    {
        string Serialize(IEnumerable<ISessionWeb> webs);
        IEnumerable<ISessionWeb> Deserialize(string data);
    }
}