using MediatR;
using RentACarAPI.Application.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Queries.Car.GetAllCar
{
    public class GetAllCarQueryRequest: IRequest<GetAllCarQueryResponse>
    {
        // public Pagination Pagination { get; set; }

        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;

    }
}
