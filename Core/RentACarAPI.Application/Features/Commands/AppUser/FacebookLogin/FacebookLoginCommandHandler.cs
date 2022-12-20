using Google.Apis.Http;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RentACarAPI.Application.Abstractions.Token;
using RentACarAPI.Application.DTOs.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Common.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly HttpClient _httpClient;

        public FacebookLoginCommandHandler(System.Net.Http.IHttpClientFactory httpClientFactory, ITokenHandler tokenHandler, UserManager<Domain.Entities.Common.Identity.AppUser> userManager)
        {
            _httpClient = httpClientFactory.CreateClient();
            _tokenHandler = tokenHandler;
            _userManager = userManager;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
           
           

        }
    }
}
