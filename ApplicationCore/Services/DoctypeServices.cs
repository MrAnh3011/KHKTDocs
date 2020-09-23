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
        public async Task<IEnumerable<apec_khktdocs_folder>> GetListDocType()
        {
            var lstDoctype = await _doctypeRepository.GetAllAsync().ConfigureAwait(false);

            return lstDoctype;
        }
    }
}
