using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<apec_khktdocs_folder>> GetChildFoldersById(string id)
        {
            List<apec_khktdocs_folder> lst = new List<apec_khktdocs_folder>();

            string where = $"where parent = '{id}'";
            var lstFolders = await _doctypeRepository.SelectQuery(where);
            if (lstFolders.Count() != 0)
            {
                foreach (var item in lstFolders)
                {
                    lst.AddRange(await GetChildFoldersById(item.id.ToString()));
                }
            }
            var folder = await _doctypeRepository.GetByIdAsync(int.Parse(id));
            lst.Add(folder);

            return lst;
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
                    return folder.id;
                }
                else
                {
                    var result = await _doctypeRepository.SaveFolder(folder);
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
