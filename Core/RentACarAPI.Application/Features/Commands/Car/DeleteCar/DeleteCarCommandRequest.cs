using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Commands.Car.DeleteCar
{
    public class DeleteCarCommandRequest:IRequest<DeleteCarCommandResponse>
    {
        public string Id { get; set; }
    }
}
