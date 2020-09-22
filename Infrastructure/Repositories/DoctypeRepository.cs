using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class DoctypeRepository : GenericRepositoryAsync<DocumentType>, IDoctypeRepository
    {
        public DoctypeRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
