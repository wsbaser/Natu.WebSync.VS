using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb
{
    public interface ISessionWebPovider {
        Task<IEnumerable<ISessionWeb>> GetSessionWebsAsync(DocumentId changedDocumentId);
        Task<IEnumerable<ISessionWeb>> GetSessionWebsAsync(bool useCache);
    }
}