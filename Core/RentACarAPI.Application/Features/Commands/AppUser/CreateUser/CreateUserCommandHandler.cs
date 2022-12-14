using MediatR;
using Microsoft.AspNetCore.Identity;
using RentACarAPI.Application.Exceptions;

namespace RentACarAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<RentACarAPI.Domain.Entities.Common.Identity.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<RentACarAPI.Domain.Entities.Common.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result =  await _userManager.CreateAsync(new(){
                Id = Guid.NewGuid().ToString(),    
                UserName = request.Username,
                Email = request.Email,
                NameSurname = request.Name,

            }, request.Password);
            CreateUserCommandResponse createUserCommandResponse = new CreateUserCommandResponse()
            {
                Succeeded = result.Succeeded,
            };

            if (result.Succeeded)
                createUserCommandResponse.Message = "User Created";
            else
                foreach (var item in result.Errors)
                    createUserCommandResponse.Message += $"{item.Code} - {item.Description}\n";
            return createUserCommandResponse;

            //throw new UserCreateFailedException();
            
        }
    }
}
