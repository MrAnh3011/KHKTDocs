using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DoctypeRepository : GenericRepositoryAsync<apec_khktdocs_folder>, IDoctypeRepository
    {
        public DoctypeRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<int> SaveFolder(apec_khktdocs_folder folder)
        {
            try
            {
                var query = "INSERT INTO apec_khktdocs_folder(ID, PARENT, TEXT, CREATED_USER, MODIFIED_USER)" +
                    " VALUES (SEQ_KHKTDOCS_FOLDER.NEXTVAL, :PARENT, :TEXT, :CREATED_USER, :MODIFIED_USER) returning ID into :id";
                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    conn.Open();

                    var param = new DynamicParameters(folder);
                    param.Output(folder, x => x.id);

                    var result = await conn.ExecuteAsync(query, param);
                    var id = param.Get<int>("id");
                    return id;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
