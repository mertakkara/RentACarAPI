using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Repositories
{
    public interface IFileWriteRepository: IWriteRepository<RentACarAPI.Domain.Entities.File> 
    {
    }
}
