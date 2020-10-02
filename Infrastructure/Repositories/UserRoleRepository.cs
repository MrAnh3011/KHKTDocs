using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRoleRepository : GenericRepositoryAsync<apec_khktdocs_userrole>, IUserRoleRepository
    {
        public UserRoleRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> SaveUserRole(apec_khktdocs_userrole role)
        {
            try
            {
                var query = "INSERT INTO apec_khktdocs_userrole(id, username, isadmin, isapprove, isdelete, issuperadmin, isaccess)" +
                        " VALUES(seq_khktdocs_userrole.NEXTVAL, :username, :isadmin, :isapprove, :isdelete, 0, :isaccess)" +
                        " returning ID into :id";

                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    conn.Open();

                    var param = new DynamicParameters(role);
                    param.Output(role, x => x.id);

                    var result = await conn.ExecuteAsync(query, param).ConfigureAwait(false);
                    var id = param.Get<int>("id");

                    return id;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<apec_khktdocs_userrole> GetUserRoleByUserName(string username)
        {
            try
            {
                var query = "SELECT * FROM apec_khktdocs_userrole WHERE username = :username AND ROWNUM = 1";

                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    conn.Open();
                    var result = await conn.QueryAsync<apec_khktdocs_userrole>(query, new { username }).ConfigureAwait(false);

                    return result.SingleOrDefault();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
