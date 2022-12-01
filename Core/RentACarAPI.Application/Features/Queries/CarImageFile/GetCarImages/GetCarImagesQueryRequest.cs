using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Queries.CarImageFile.GetCarImages
{
    public class GetCarImagesQueryRequest: IRequest<List<GetCarImagesQueryResponse>>
    {
        public string Id { get; set; }
    }
}
