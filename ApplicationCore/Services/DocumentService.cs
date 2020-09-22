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

        public async Task SaveDocument(Document document)
        {
            if(!String.IsNullOrEmpty(document.DocumentId))
            {
                var entity = await _documentRepository.GetByIdAsync(document.DocumentId).ConfigureAwait(false);

                entity.DocumentTypeId = document.DocumentTypeId;
                entity.DocumentUrl = document.DocumentUrl;

                await _documentRepository.UpdateAsync(entity).ConfigureAwait(false);
            }

            document.DocumentId = "SQL_DOCS.NEXTVAL";
            await _documentRepository.AddAsync(document).ConfigureAwait(false);
        }

        public async Task DeleteDocument(string id)
        {
            await _documentRepository.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<Document> GetDocumentById(string id)
        {
            var document = await _documentRepository.GetByIdAsync(id).ConfigureAwait(false);

            if(document != null)
            {
                return document;
            }
            else
            {
                return new Document();
            }
        }
    }
}
