using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Queries.Car.GetAllCar
{
    public class GetAllCarQueryResponse
    {
        public int TotalCount { get; set; }
        public object Cars { get; set; }
    }
}
