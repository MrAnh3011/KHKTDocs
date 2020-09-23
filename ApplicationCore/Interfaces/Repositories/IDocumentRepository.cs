using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IDocumentRepository : IGenericRepositoryAsync<apec_khktdocs_document>
    {
        Task<int> SaveDocument(apec_khktdocs_document document);
    }
}
