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
    public class InvoiceFileReadRepository : ReadRepository<InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceFileReadRepository(RentACarAPIDbContext context) : base(context)
        {
        }
    }
}
