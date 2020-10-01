using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepositoryAsync<Users>
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<Users> GetUsersByUserName(string username);
        Task<Users> GetApprover();
        Task<IEnumerable<Users>> GetAllUsersWithDepart();
    }
}
