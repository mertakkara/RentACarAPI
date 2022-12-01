using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentACarAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Queries.CarImageFile.GetCarImages
{
    public class GetCarImagesQueryHandler : IRequestHandler<GetCarImagesQueryRequest, List<GetCarImagesQueryResponse>>
    {
        readonly ICarReadRepository carReadRepository;
        readonly IConfiguration  configuration;

        public GetCarImagesQueryHandler(ICarReadRepository carReadRepository, IConfiguration configuration)
        {
            this.carReadRepository = carReadRepository;
            this.configuration = configuration;
        }

        public async Task<List<GetCarImagesQueryResponse>> Handle(GetCarImagesQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Car? car = await carReadRepository.Table.Include(c => c.CarImageFiles).FirstOrDefaultAsync(c => c.Id == Guid.Parse(request.Id));
            return car?.CarImageFiles.Select(c => new GetCarImagesQueryResponse
            {
                Path = $"{configuration["BaseStorageUrl"]}/{c.Path}",
                FileName = c.FileName,
                Id = c.Id
            }).ToList();
        }
    }
}
