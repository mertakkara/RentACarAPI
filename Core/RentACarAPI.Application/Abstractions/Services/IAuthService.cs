using RentACarAPI.Application.Abstractions.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Application.Abstractions.Services
{
    public interface IAuthService: IInternalAuthentication,IExternalAuthentication
    {

   
      
    }
}
