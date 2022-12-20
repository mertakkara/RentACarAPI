using MediatR;
using Microsoft.AspNetCore.Identity;
using RentACarAPI.Application.Abstractions.Services;
using RentACarAPI.Application.DTOs.User;
using RentACarAPI.Application.Exceptions;

namespace RentACarAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
           CreateUserResponse response = await _userService.CreateAsync(new()
            {
                Email = request.Email,
                Name = request.Name,    
                Password = request.Password,
                PasswordAgain = request.PasswordAgain,
                Username = request.Username,    
            });
            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded, 
            };

            //throw new UserCreateFailedException();
            
        }
    }
}
