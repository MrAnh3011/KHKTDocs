using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class DoctypeRepository : GenericRepositoryAsync<apec_khktdocs_folder>, IDoctypeRepository
    {
        public DoctypeRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
