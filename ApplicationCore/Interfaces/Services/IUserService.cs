using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTOs>> GetAllUserWithDepart();
        Task<Users> GetAppover();
    }
}
