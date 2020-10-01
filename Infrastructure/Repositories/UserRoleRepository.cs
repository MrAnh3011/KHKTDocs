using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRoleRepository : GenericRepositoryAsync<apec_khktdocs_role>, IUserRoleRepository
    {
        public UserRoleRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> SaveUserRole(apec_khktdocs_role role)
        {
            try
            {
                var query = "INSERT INTO apec_khktdocs_role(id, username, isadmin, isapprove, isdelete)" +
                        " VALUES(seq_khktdocs_userrole.NEXTVAL, :username, :isadmin, :isapprove, :isdelete)" +
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
    }
}
