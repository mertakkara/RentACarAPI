using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Commands.CarImageFile.RemoveCarImage
{
    public class RemoveCarImageCommandRequest : IRequest<RemoveCarImageCommandResponse>
    {
        public string Id { get; set; }
        public string? ImageId { get; set; }
    }
}
