using MediatR;
using Microsoft.Extensions.Logging;
using RentACarAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Commands.Car.UpdateCar
{
    public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommandRequest, UpdateCarCommandResponse>
    {
        readonly ICarReadRepository carReadRepository;
        readonly ICarWriteRepository carWriteRepository;
        readonly ILogger<UpdateCarCommandHandler> _logger;

        public UpdateCarCommandHandler(ICarWriteRepository carWriteRepository, ICarReadRepository carReadRepository, ILogger<UpdateCarCommandHandler> logger)
        {
            this.carWriteRepository = carWriteRepository;
            this.carReadRepository = carReadRepository;
            _logger = logger;
        }

        public async Task<UpdateCarCommandResponse> Handle(UpdateCarCommandRequest request, CancellationToken cancellationToken)
        {
            RentACarAPI.Domain.Entities.Car car = await carReadRepository.GetByIdAsync(request.Id);
            car.Stock = request.Stock;
            car.Name = request.Name;
            car.Price = request.Price;
            await carWriteRepository.SaveAsync();
            _logger.LogInformation("Car Updated");
            return new();
        }
    }
}
