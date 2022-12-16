using MediatR;
using Microsoft.AspNetCore.Identity;
using RentACarAPI.Application.Abstractions.Token;
using RentACarAPI.Application.DTOs;
using RentACarAPI.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.Common.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Common.Identity.AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(SignInManager<Domain.Entities.Common.Identity.AppUser> signInManager, UserManager<Domain.Entities.Common.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Common.Identity.AppUser user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            if(user == null)    
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            if (user == null)
                throw new NotFoundUserException("Username Or Password is Wrong");
          SignInResult result =  await _signInManager.CheckPasswordSignInAsync(user,request.Password,false);
            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(5);
                return new LoginUserSuccessCommandResponse()
                {
                    Token = token
                };
            }
            throw new AuthenticationErrorException();
            //return new LoginUserErrorCommandResponse()
            //{
            //    Message = "Username Or Password is Wrong"
            //};
        }
    }
}
