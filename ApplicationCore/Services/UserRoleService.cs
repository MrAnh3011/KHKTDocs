using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        public UserRoleService(IUserRoleRepository userRoleRepository, IUserRepository userRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
        }

        public async Task DeleteUserRole(int id)
        {
            try
            {
                await _userRoleRepository.DeleteAsync(id);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<UserRoleDTOs>> GetAllUserRole()
        {
            try
            {
                var result = await _userRoleRepository.GetAllAsync().ConfigureAwait(false);
                List<UserRoleDTOs> listRole = new List<UserRoleDTOs>();

                foreach (var item in result)
                {
                    if (item.username == "anhpt") continue;
                    var user = await _userRepository.GetUsersByUserName(item.username);

                    var role = new UserRoleDTOs
                    {
                        id = item.id,
                        username = item.username,
                        fullname = user.display_name,
                        isadmin = item.isadmin,
                        isapprove = item.isapprove,
                        isdelete = item.isdelete,
                        isaccess = item.isaccess
                    };

                    listRole.Add(role);
                }

                return listRole;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<UserRoleDTOs> GetUserRoleById(int id)
        {
            try
            {
                var result = await _userRoleRepository.GetByIdAsync(id).ConfigureAwait(false);

                if (result.username == "anhpt") return null;

                var user = await _userRepository.GetUsersByUserName(result.username);

                var userRole = new UserRoleDTOs
                {
                    id = result.id,
                    username = result.username,
                    fullname = user.display_name,
                    isadmin = result.isadmin,
                    isapprove = result.isapprove,
                    isdelete = result.isdelete,
                    isaccess = result.isaccess,
                    issuperadmin = result.issuperadmin
                };

                return userRole;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<UserRoleDTOs> GetUserRoleByUserName(string username)
        {
            try
            {
                var result = await _userRoleRepository.GetUserRoleByUserName(username).ConfigureAwait(false);
                if (result == null) return null;
                var user = await _userRepository.GetUsersByUserName(username);

                var userRole = new UserRoleDTOs
                {
                    id = result.id,
                    username = result.username,
                    fullname = user.display_name,
                    isadmin = result.isadmin,
                    isapprove = result.isapprove,
                    isdelete = result.isdelete,
                    isaccess = result.isaccess,
                    issuperadmin = result.issuperadmin
                };

                return userRole;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<int> SaveUserRole(apec_khktdocs_userrole role)
        {
            try
            {
                if (role.id != 0)
                {
                    var entity = await _userRoleRepository.GetByIdAsync(role.id).ConfigureAwait(false);

                    entity.isadmin = role.isadmin;
                    entity.isapprove = role.isapprove;
                    entity.isdelete = role.isdelete;
                    entity.username = role.username;
                    entity.isaccess = role.isaccess;

                    await _userRoleRepository.UpdateAsync(entity).ConfigureAwait(false);

                    return role.id;
                }
                else
                {
                    var isexists = await GetUserRoleByUserName(role.username).ConfigureAwait(false);
                    if (isexists == null)
                    {
                        var result = await _userRoleRepository.SaveUserRole(role).ConfigureAwait(false);
                        return result;
                    }
                    else return -1;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
