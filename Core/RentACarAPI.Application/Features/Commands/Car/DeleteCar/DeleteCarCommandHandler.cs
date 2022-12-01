using MediatR;
using RentACarAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Commands.Car.DeleteCar
{
    public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommandRequest, DeleteCarCommandResponse>
    {
        readonly ICarWriteRepository carWriteRepository;

        public DeleteCarCommandHandler(ICarWriteRepository carWriteRepository)
        {
            this.carWriteRepository = carWriteRepository;
        }

        public async Task<DeleteCarCommandResponse> Handle(DeleteCarCommandRequest request, CancellationToken cancellationToken)
        {
            await carWriteRepository.RemoveAsync(request.Id);
            await carWriteRepository.SaveAsync();
            return new();
        }
    }
}
