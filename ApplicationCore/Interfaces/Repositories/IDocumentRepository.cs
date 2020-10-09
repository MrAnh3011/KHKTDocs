using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IDocumentRepository : IGenericRepositoryAsync<apec_khktdocs_document>
    {
        Task<int> SaveDocument(apec_khktdocs_document document);
        Task<int> ApproveDocument(int id, string approver);
    }
}
