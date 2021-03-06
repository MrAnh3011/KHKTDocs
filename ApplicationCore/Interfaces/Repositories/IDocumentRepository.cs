﻿using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IDocumentRepository : IGenericRepositoryAsync<apec_khktdocs_document>
    {
        Task<int> SaveDocument(apec_khktdocs_document document);
        Task<int> ApproveDocument(int id, string approver);
        Task SendMail(MailSenderDTOs mailInfo);
        Task EditNote(int id, string note);
    }
}
