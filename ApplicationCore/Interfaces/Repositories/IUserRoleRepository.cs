using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IUserRoleRepository : IGenericRepositoryAsync<apec_khktdocs_userrole>
    {
        Task<int> SaveUserRole(apec_khktdocs_userrole role);
    }
}
