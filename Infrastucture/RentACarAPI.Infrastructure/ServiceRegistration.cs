using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACarAPI.Application.Abstractions.Storage;
using RentACarAPI.Application.Abstractions.Token;
using RentACarAPI.Infrastructure.Enums;
using RentACarAPI.Infrastructure.Services;
using RentACarAPI.Infrastructure.Services.Storage;
using RentACarAPI.Infrastructure.Services.Storage.Azure;
using RentACarAPI.Infrastructure.Services.Storage.Local;
using RentACarAPI.Infrastructure.Services.Token;
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

            
            services.AddScoped<IStorageService,StorageService>();
            services.AddScoped<ITokenHandler,TokenHandler>();

        }
        public static void AddStorage<T>(this IServiceCollection services) where T : Storage,IStorage // alttaki yerine bunu tercih ediyoruz
        {
           
            services.AddScoped<IStorage, T>();

        }

        public static void AddStorage<T>(this IServiceCollection services,StorageType storageType) 
        {
            switch (storageType)
            {
                case StorageType.Local:
                    services.AddScoped<IStorage,  LocalStorage>();
                    break;
                case StorageType.Azure:
                    services.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
           

        }
    }
}
