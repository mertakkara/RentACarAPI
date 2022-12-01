using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Commands.CarImageFile.RemoveCarImage
{
    public class RemoveCarImageCommandHandler : IRequestHandler<RemoveCarImageCommandRequest, RemoveCarImageCommandResponse>
    {
        readonly ICarReadRepository carReadRepository;
        readonly ICarWriteRepository carWriteRepository;

        public RemoveCarImageCommandHandler(ICarWriteRepository carWriteRepository, ICarReadRepository carReadRepository)
        {
            this.carWriteRepository = carWriteRepository;
            this.carReadRepository = carReadRepository;
        }

        public async Task<RemoveCarImageCommandResponse> Handle(RemoveCarImageCommandRequest request, CancellationToken cancellationToken)
        {
           Domain.Entities.Car? car = await carReadRepository.Table.Include(c => c.CarImageFiles).FirstOrDefaultAsync(c => c.Id == Guid.Parse(request.Id));
            Domain.Entities.CarImageFile? carImageFile = car?.CarImageFiles.FirstOrDefault(c => c.Id == Guid.Parse(request.ImageId));
            
            if(carImageFile != null)
            car?.CarImageFiles.Remove(carImageFile);
            await carWriteRepository.SaveAsync();
            return new();
        }
    }
}
