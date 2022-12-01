using MediatR;
using RentACarAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Queries.Car.GetAllCar
{
    public class GetAllCarQueryHandler : IRequestHandler<GetAllCarQueryRequest, GetAllCarQueryResponse>
    {
        readonly ICarReadRepository carReadRepository;
        public GetAllCarQueryHandler(ICarReadRepository carReadRepository)
        {
            this.carReadRepository = carReadRepository;
        }
        public async Task<GetAllCarQueryResponse> Handle(GetAllCarQueryRequest request, CancellationToken cancellationToken)
        {
            var totalCount = carReadRepository.GetAll(false).Count();
            var cars = carReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDateTime,
                p.UpdatedDate
            }
           ).ToList();

            return  new GetAllCarQueryResponse() 
            {
                Cars = cars,    
                TotalCount = totalCount 
            };
        }
    }
}
