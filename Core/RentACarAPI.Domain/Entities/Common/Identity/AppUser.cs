
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Domain.Entities.Common.Identity
{
    public class AppUser: IdentityUser<string>
    {
        public string NameSurname { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenLifeTime { get; set; }
    }
}
