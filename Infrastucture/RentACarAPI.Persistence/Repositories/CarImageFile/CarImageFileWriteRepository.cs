using RentACarAPI.Application.Repositories;
using RentACarAPI.Domain.Entities;
using RentACarAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Persistence.Repositories
{
    public class CarImageFileWriteRepository : WriteRepository<CarImageFile>, ICarImageFileWriteRepository
    {
        public CarImageFileWriteRepository(RentACarAPIDbContext context) : base(context)
        {
        }
    }
}
