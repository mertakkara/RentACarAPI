using MediatR;
using Microsoft.AspNetCore.Identity;
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

        public LoginUserCommandHandler(SignInManager<Domain.Entities.Common.Identity.AppUser> signInManager, UserManager<Domain.Entities.Common.Identity.AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
               
            }
               

            return new();
        }
    }
}
