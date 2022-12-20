using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RentACarAPI.Application.Abstractions.Services;
using RentACarAPI.Application.Abstractions.Token;
using RentACarAPI.Application.DTOs;
using RentACarAPI.Application.DTOs.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RentACarAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<Domain.Entities.Common.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;

        public AuthService(System.Net.Http.IHttpClientFactory httpClientFactory, ITokenHandler tokenHandler, UserManager<Domain.Entities.Common.Identity.AppUser> userManager, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _tokenHandler = tokenHandler;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<Token> FacebookLoginAsync(string authToken)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");
            FacebookAccessTokenResponse? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);
            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse?.AccessToken}");
            FacebookUserAccessTokenValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);
            if (validation?.Data.IsValid != null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

                FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);


                var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
                Domain.Entities.Common.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                bool result = user != null;
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(userInfo?.Email);
                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = userInfo?.Email,
                            UserName = userInfo?.Email,
                            NameSurname = userInfo?.Name
                        };
                        var identityResult = await _userManager.CreateAsync(user);
                        result = identityResult.Succeeded;
                    }
                }
                if (result)
                {
                    await _userManager.AddLoginAsync(user, info);
                    Token token = _tokenHandler.CreateAccessToken(5);
                    return token;
                }
                throw new Exception("Invalid authentication");
            }
        }

        public Task<Token> GoogleLoginAsync(string idToken)
        {
            throw new NotImplementedException();
        }

        public Task LoginAsync()
        {
            throw new NotImplementedException();
        }
    }
}
