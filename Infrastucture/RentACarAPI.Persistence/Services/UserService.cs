using Microsoft.AspNetCore.Identity;
using RentACarAPI.Application.Abstractions.Services;
using RentACarAPI.Application.DTOs.User;
using RentACarAPI.Application.Exceptions;
using RentACarAPI.Domain.Entities.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<RentACarAPI.Domain.Entities.Common.Identity.AppUser> _userManager;

        public UserService(UserManager<RentACarAPI.Domain.Entities.Common.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                NameSurname = model.Name,

            }, model.Password);
            CreateUserResponse createUserCommandResponse = new CreateUserResponse()
            {
                Succeeded = result.Succeeded,
            };

            if (result.Succeeded)
                createUserCommandResponse.Message = "User Created";
            else
                foreach (var item in result.Errors)
                    createUserCommandResponse.Message += $"{item.Code} - {item.Description}\n";
            return createUserCommandResponse;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTpkeLifeTime,int refreshTokenLifeTime)
        {
            if(user != null)
            {
                user.RefreshToken = refreshToken;   
                user.RefreshTokenLifeTime = accessTpkeLifeTime.AddSeconds(refreshTokenLifeTime);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();
        }
    }
}
