using ApplicationCore.Entities;
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

        public async Task<int> SaveDocument(apec_khktdocs_document document)
        {
            try
            {
                string query = "insert into apec_khktdocs_document (DOCUMENTID, DOCUMENT_NAME, DISPLAY_NAME, DOCUMENT_DESCRIPTION, CREATED_USER, STATUS, CREATED_DATE, DOCUMENT_EXTENSION, DOCUMENT_FOLDER_ID)";
                query += $"values(SEQ_KHKTDOCS_DOC.NEXTVAL, :DOCUMENT_NAME, :DISPLAY_NAME, :DOCUMENT_DESCRIPTION, :CREATED_USER, :STATUS, :CREATED_DATE, :DOCUMENT_EXTENSION, :DOCUMENT_FOLDER_ID)";

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
