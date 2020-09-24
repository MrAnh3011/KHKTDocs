using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepositoryAsync<Users>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Users>> GetAllUsersWithDepart()
        {
            try
            {
                var query = "SELECT a.user_id, a.username, a.display_name, a.email, a.staff_id, ad.department_name FROM users a";
                query += " INNER JOIN apec_staff ct ON a.staff_id = ct.staff_id";
                query += " INNER JOIN apec_department ad ON ct.depa_id = ad.department_id";

                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    conn.Open();
                    var data = await conn.QueryAsync<Users>(query);
                    return data;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<Users> GetApprover()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Users> GetUsersByUserName(string username)
        {
            try
            {
                string query = "SELECT a.user_id, a.username, a.display_name, a.email, a.staff_id FROM USERS a WHERE a.username = :username";
                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    conn.Open();
                    var data = await conn.QueryAsync<Users>(query, new { username });
                    return data.FirstOrDefault();
                }
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
    }
}
