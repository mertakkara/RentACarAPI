using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Queries.Car.GetByIdCar
{
    public class GetByIdCarQueryRequest : IRequest<GetByIdCarQueryResponse>
    {
        public string id { get; set; }
    }
}
