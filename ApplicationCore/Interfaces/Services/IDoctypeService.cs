using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IDoctypeService
    {
        Task<IEnumerable<apec_khktdocs_folder>> GetListDocType();
        Task<int> SaveFolder(apec_khktdocs_folder folder);
        Task DeleteFolder(int id);
        Task<IEnumerable<apec_khktdocs_folder>> GetChildFoldersById(string id);
    }
}
