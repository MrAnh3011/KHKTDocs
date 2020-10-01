using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IUserRoleService
    {
        Task<int> SaveUserRole(apec_khktdocs_userrole role);
        Task DeleteUserRole(int id);
        Task<IEnumerable<UserRoleDTOs>> GetAllUserRole();
        Task<UserRoleDTOs> GetUserRoleById(int id);
    }
}
