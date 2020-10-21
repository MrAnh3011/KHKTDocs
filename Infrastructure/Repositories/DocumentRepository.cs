using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DocumentRepository : GenericRepositoryAsync<apec_khktdocs_document>, IDocumentRepository
    {
        public DocumentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> ApproveDocument(int id, string approver)
        {
            string query = $"UPDATE apec_khktdocs_document a SET a.status = {(int)DocumentStatus.Approved}, a.APPROVE_DATE = CURRENT_DATE, a.APPROVER = '{approver}' WHERE a.id = :id";
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var result = await conn.ExecuteAsync(query, new { id, approver });

                return result;
            }
        }

        public async Task EditNote(int id, string note)
        {
            string query = "UPDATE apec_khktdocs_document SET DOCUMENT_DESCRIPTION = :note WHERE ID = :id";
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                await conn.ExecuteAsync(query, new { id, note });
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

        public async Task SendMail(MailSenderDTOs mailInfo)
        {
            string query = "sp_send_approve_mail";

            var param = new DynamicParameters();
            param.Add("pv_approver", dbType: DbType.String, value: mailInfo.approver);
            param.Add("pv_requester", dbType: DbType.String, value: mailInfo.requester);
            param.Add("pv_status", dbType: DbType.String, value: mailInfo.status);
            param.Add("pv_folder", dbType: DbType.String, value: mailInfo.folder);
            param.Add("pv_docname", dbType: DbType.String, value: mailInfo.docname);
            param.Add("pv_docdate", dbType: DbType.Date, value: mailInfo.docdate);
            param.Add("pv_note", dbType: DbType.String, value: mailInfo.note);
            param.Add("pv_link", dbType: DbType.String, value: mailInfo.link);
            param.Add("pv_toemail", dbType: DbType.String, value: mailInfo.approverMail);
            param.Add("pv_sendermail", dbType: DbType.String, value: mailInfo.sendermail);
            param.Add("SESSIONINFO_USERNAME", dbType: DbType.String, value: "anhpt");


            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();

                var result = await conn.ExecuteAsync(query, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
