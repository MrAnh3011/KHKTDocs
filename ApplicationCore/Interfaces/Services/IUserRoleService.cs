using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IUserRoleService
    {
        Task<int> SaveUserRole(apec_khktdocs_role role);
        Task DeleteUserRole(int id);
        Task<IEnumerable<apec_khktdocs_role>> GetAllUserRole();
        Task<apec_khktdocs_role> GetUserRoleById(int id);
    }
}
