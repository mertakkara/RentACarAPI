using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentACarAPI.Application.Features.Commands.AppUser.CreateUser;
using RentACarAPI.Application.Features.Commands.AppUser.FacebookLogin;
using RentACarAPI.Application.Features.Commands.AppUser.GoogleLogin;
using RentACarAPI.Application.Features.Commands.AppUser.LoginUser;

namespace RentACarAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task <IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response =  await _mediator.Send(createUserCommandRequest);
            return Ok(response);
        }
      
    }
}
