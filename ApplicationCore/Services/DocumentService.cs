using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        public DocumentService( IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task SaveDocument(apec_khktdocs_document document)
        {
            try
            {
                if (document.documentid != 0)
                {
                    var entity = await _documentRepository.GetByIdAsync(document.documentid).ConfigureAwait(false);

                    entity.document_name = document.document_name;
                    entity.display_name = document.display_name;
                    entity.document_description = document.document_description;
                    entity.created_user = document.created_user;
                    entity.status = document.status;
                    entity.approve_date = document.approve_date;
                    entity.document_extension = document.document_extension;
                    entity.document_folder_id = document.document_folder_id;
                    entity.modified_date = document.modified_date;

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

            if(document != null)
            {
                return document;
            }
            else
            {
                return new apec_khktdocs_document();
            }
        }

        public async Task<IEnumerable<apec_khktdocs_document>> GetAllDocument()
        {
            var lstDocs = await _documentRepository.GetAllAsync().ConfigureAwait(false);
            return lstDocs;
        }
    }
}
