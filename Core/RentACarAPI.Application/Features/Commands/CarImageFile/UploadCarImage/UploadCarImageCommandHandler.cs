using MediatR;
using RentACarAPI.Application.Abstractions.Storage;
using RentACarAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Commands.CarImageFile.UploadCarImage
{
    public class UploadCarImageCommandHandler : IRequestHandler<UploadCarImageCommandRequest, UploadCarImageCommandResponse>
    {
        readonly IStorageService storageService;
        readonly ICarReadRepository carReadRepository;
        readonly ICarImageFileWriteRepository carImageFileWriteRepository;

        public UploadCarImageCommandHandler(IStorageService storageService, ICarReadRepository carReadRepository, ICarImageFileWriteRepository carImageFileWriteRepository)
        {
            this.storageService = storageService;
            this.carReadRepository = carReadRepository;
            this.carImageFileWriteRepository = carImageFileWriteRepository;
        }

      
        public async Task<UploadCarImageCommandResponse> Handle(UploadCarImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> result = await storageService.UploadAsync("photo-images", request.Files);

            Domain.Entities.Car car = await carReadRepository.GetByIdAsync(request.Id);


            await carImageFileWriteRepository.AddRangeAsync(result.Select(r => new Domain.Entities.CarImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = storageService.StorageName,
                Cars = new List<Domain.Entities.Car>() { car }
            }).ToList());

            await carImageFileWriteRepository.SaveAsync();
            return new();

        }
    }
}
