using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IDocumentService
    {
        Task SaveDocument(apec_khktdocs_document document);
        Task DeleteDocument(int id);
        Task<apec_khktdocs_document> GetDocumentById(int id);
        Task<IEnumerable<DocumentDetailDTO>> GetAllDocument();
        Task ApproveDocument(int id);
        Task<IEnumerable<DocumentDetailDTO>> GetDocsByFolderId(string id);
    }
}
