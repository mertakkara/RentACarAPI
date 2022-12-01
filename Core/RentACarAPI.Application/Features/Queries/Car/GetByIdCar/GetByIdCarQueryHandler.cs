using MediatR;
using RentACarAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Queries.Car.GetByIdCar
{
    public class GetByIdCarQueryHandler : IRequestHandler<GetByIdCarQueryRequest, GetByIdCarQueryResponse>
    {
        readonly ICarReadRepository carReadRepository;

        public GetByIdCarQueryHandler(ICarReadRepository carReadRepository)
        {
            this.carReadRepository = carReadRepository;
        }

        public async Task<GetByIdCarQueryResponse> Handle(GetByIdCarQueryRequest request, CancellationToken cancellationToken)
        {
            RentACarAPI.Domain.Entities.Car car = await carReadRepository.GetByIdAsync(request.id, false);
            return new(){
                Name = car.Name,
                Price = car.Price,
                Stock = car.Stock
            };
        }
    }
}
