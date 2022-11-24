using RentACarAPI.Application.Repositories;
using RentACarAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<RentACarAPI.Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(RentACarAPIDbContext context) : base(context)
        {
        }
    }
}
