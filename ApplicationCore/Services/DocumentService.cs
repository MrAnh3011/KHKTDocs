using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDoctypeRepository _doctypeRepository;
        public DocumentService(IDocumentRepository documentRepository, IUserRepository userRepository, IDoctypeRepository doctypeRepository)
        {
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _doctypeRepository = doctypeRepository;
        }

        public async Task SaveDocument(apec_khktdocs_document document)
        {
            try
            {
                if (document.id != 0)
                {
                    var entity = await _documentRepository.GetByIdAsync(document.id).ConfigureAwait(false);

                    entity.document_name = document.document_name;
                    entity.display_name = document.display_name;
                    entity.document_description = document.document_description;
                    entity.created_user = document.created_user;
                    entity.status = document.status;
                    entity.approve_date = document.approve_date;
                    entity.document_extension = document.document_extension;
                    entity.document_folder_id = document.document_folder_id;
                    entity.modified_date = document.modified_date;
                    entity.document_receiver = document.document_receiver;

                    await _documentRepository.UpdateAsync(entity).ConfigureAwait(false);
                }

                await _documentRepository.SaveDocument(document).ConfigureAwait(false);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task DeleteDocument(int id)
        {
            await _documentRepository.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<apec_khktdocs_document> GetDocumentById(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (document != null)
            {
                return document;
            }
            else
            {
                return new apec_khktdocs_document();
            }
        }

        public async Task<IEnumerable<DocumentDetailDTO>> GetAllDocument()
        {
            List<DocumentDetailDTO> lstDocDTO = new List<DocumentDetailDTO>();
            DocumentDetailDTO itemDocs;
            var lstDocs = await _documentRepository.GetAllAsync().ConfigureAwait(false);
            foreach (var item in lstDocs)
            {
                var username = await _userRepository.GetUsersByUserName(item.created_user).ConfigureAwait(false);
                var foldername = await _doctypeRepository.GetByIdAsync(item.document_folder_id).ConfigureAwait(false);

                itemDocs = new DocumentDetailDTO
                {
                    id = item.id,
                    document_name = item.document_name,
                    document_description = item.document_description,
                    document_extension = item.document_extension,
                    created_user = username.display_name,
                    created_date = item.created_date,
                    approve_date = item.approve_date,
                    folder_name = foldername.text,
                    display_name = item.display_name,
                    status = ((DocumentStatus)item.status).ToString(),
                    document_receiver = item.document_receiver
                };
                lstDocDTO.Add(itemDocs);
            }
            return lstDocDTO;
        }

        public async Task ApproveDocument(int id)
        {
            await _documentRepository.ApproveDocument(id).ConfigureAwait(false);
        }
    }
}
