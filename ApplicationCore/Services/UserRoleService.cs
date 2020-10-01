using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
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

        public async Task<IEnumerable<apec_khktdocs_role>> GetAllUserRole()
        {
            var result = await _userRoleRepository.GetAllAsync().ConfigureAwait(false);

            return result;
        }

        public async Task<apec_khktdocs_role> GetUserRoleById(int id)
        {
            var result = await _userRoleRepository.GetByIdAsync(id).ConfigureAwait(false);

            return result;
        }

        public async Task<int> SaveUserRole(apec_khktdocs_role role)
        {
            try
            {
                if(role.id != 0)
                {
                    var entity = await _userRoleRepository.GetByIdAsync(role.id).ConfigureAwait(false);

                    entity.isadmin = role.isadmin;
                    entity.isapprove = role.isapprove;
                    entity.isdelete = role.isdelete;
                    entity.username = role.username;

                    await _userRoleRepository.UpdateAsync(entity).ConfigureAwait(false);

                    return role.id;
                }
                else
                {
                    var result = await _userRoleRepository.SaveUserRole(role).ConfigureAwait(false);

                    return result;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
