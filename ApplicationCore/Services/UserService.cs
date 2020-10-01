using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserDTOs>> GetAllUserWithDepart()
        {
            List<UserDTOs> lstUsers = new List<UserDTOs>();

            var data = await _userRepository.GetAllUsersWithDepart().ConfigureAwait(false);

            foreach (var item in data)
            {
                var user = new UserDTOs
                {
                    user_id = item.user_id,
                    username = item.username,
                    display_name = item.display_name,
                    email = item.email,
                    staff_id = item.staff_id,
                    department_name = item.department_name,
                    full_name = item.display_name + " - " + item.department_name
                };
                lstUsers.Add(user);
            }

            return lstUsers;
        }

        public async Task<Users> GetAppover()
        {
            throw new NotImplementedException();
        }
    }
}
