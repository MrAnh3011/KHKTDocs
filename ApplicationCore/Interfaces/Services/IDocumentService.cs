using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IDocumentService
    {
        Task SaveDocument(Document document);
        Task DeleteDocument(string id);
        Task<Document> GetDocumentById(string id);
    }
}
