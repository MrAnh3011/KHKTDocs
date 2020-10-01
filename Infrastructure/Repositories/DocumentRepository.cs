using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DocumentRepository : GenericRepositoryAsync<apec_khktdocs_document>, IDocumentRepository
    {
        public DocumentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> ApproveDocument(int id)
        {
            string query = $"UPDATE apec_khktdocs_document a SET a.status = {(int)DocumentStatus.Approved} WHERE a.id = :id";
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var result = await conn.ExecuteAsync(query, new { id });

                return result;
            }
        }

        public async Task<int> SaveDocument(apec_khktdocs_document document)
        {
            try
            {
                string query = "insert into apec_khktdocs_document (ID, DOCUMENT_NAME, STAGE, DOCUMENT_DESCRIPTION, CREATED_USER, STATUS, CREATED_DATE, DOCUMENT_EXTENSION, DOCUMENT_FOLDER_ID, DOCUMENT_RECEIVER, DOCUMENT_AGENCY)";
                query += $"values(SEQ_KHKTDOCS_DOC.NEXTVAL, :DOCUMENT_NAME, :STAGE, :DOCUMENT_DESCRIPTION, :CREATED_USER, :STATUS, :CREATED_DATE, :DOCUMENT_EXTENSION, :DOCUMENT_FOLDER_ID, :DOCUMENT_RECEIVER, :DOCUMENT_AGENCY)";

                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    conn.Open();
                    var result = await conn.ExecuteAsync(query, document);

                    return result;
                }
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
    }
}
