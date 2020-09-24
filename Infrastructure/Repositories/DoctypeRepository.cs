using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;
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
                    " VALUES (SEQ_KHKTDOCS_FOLDER.NEXTVAL, :PARENT, :TEXT, :CREATED_USER, :MODIFIED_USER)";

                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    conn.Open();
                    var result = await conn.ExecuteAsync(query, folder);

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
