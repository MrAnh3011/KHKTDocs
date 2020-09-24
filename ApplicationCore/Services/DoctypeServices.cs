using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class DoctypeServices : IDoctypeService
    {
        private readonly IDoctypeRepository _doctypeRepository;

        public DoctypeServices(IDoctypeRepository doctypeRepository)
        {
            _doctypeRepository = doctypeRepository;
        }

        public async Task DeleteFolder(int id)
        {
            await _doctypeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<apec_khktdocs_folder>> GetListDocType()
        {
            var lstDoctype = await _doctypeRepository.GetAllAsync().ConfigureAwait(false);

            return lstDoctype;
        }

        public async Task<int> SaveFolder(apec_khktdocs_folder folder)
        {
            try
            {
                if (folder.id != 0)
                {
                    var entity = await _doctypeRepository.GetByIdAsync(folder.id).ConfigureAwait(false);

                    entity.parent = folder.parent;
                    entity.text = folder.text;
                    entity.modified_user = folder.modified_user;

                    await _doctypeRepository.UpdateAsync(entity).ConfigureAwait(false);
                }

                var result = await _doctypeRepository.SaveFolder(folder);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
