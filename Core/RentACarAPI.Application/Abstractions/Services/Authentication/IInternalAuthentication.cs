using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Abstractions.Services.Authentication
{
    public interface IInternalAuthentication
    {
        Task<DTOs.Token> LoginAsync(string usernameOrEmail,string password, int accessTokenLifetime);
        Task<DTOs.Token> RefreshTokenLoginAsync(string refreshToken);
    }
}
