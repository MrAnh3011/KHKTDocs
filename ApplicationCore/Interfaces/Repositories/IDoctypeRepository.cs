using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IDoctypeRepository : IGenericRepositoryAsync<apec_khktdocs_folder>
    {
        Task<int> SaveFolder(apec_khktdocs_folder folder);
    }
}
