using RentACarAPI.Application.DTOs.User;
using RentACarAPI.Domain.Entities.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshToken(string refreshToken,AppUser user, DateTime accessTpkeLifeTime, int refreshTokenLifeTime);
    }
}
