using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACarAPI.Application.Services;
using RentACarAPI.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/RentACarAPI.API"));
            configurationManager.AddJsonFile("appsettings.json");

            
            services.AddScoped<IFileService, FileService>();

        }
    }
}
