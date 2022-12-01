using MediatR;
using RentACarAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Commands.Car.CreateCar
{
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommandRequest, CreateCarCommandResponse>
    {
        readonly ICarWriteRepository carWriteRepository;
        public CreateCarCommandHandler(ICarWriteRepository carWriteRepository)
        {
            this.carWriteRepository = carWriteRepository;
        }
        public async Task<CreateCarCommandResponse> Handle(CreateCarCommandRequest request, CancellationToken cancellationToken)
        {
            await carWriteRepository.AddAsync(new()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,

            });
            await carWriteRepository.SaveAsync();
            return new();
        }
    }
}
