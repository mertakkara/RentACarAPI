using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACarAPI.Application.Abstractions.Services;
using RentACarAPI.Application.Abstractions.Services.Authentication;
using RentACarAPI.Application.Repositories;
using RentACarAPI.Domain.Entities.Common.Identity;
using RentACarAPI.Persistence.Contexts;
using RentACarAPI.Persistence.Repositories;
using RentACarAPI.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/RentACarAPI.API"));
            configurationManager.AddJsonFile("appsettings.json");

            services.AddDbContext<RentACarAPIDbContext>(options => options.UseNpgsql(configurationManager.GetConnectionString("PostgreSQL")));
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;    
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                
            }).AddEntityFrameworkStores<RentACarAPIDbContext>();
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<ICarWriteRepository, CarWriteRepository>();
            services.AddScoped<ICarReadRepository, CarReadRepository>();
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository,InvoiceFileWriteRepository>();
            services.AddScoped<ICarImageFileReadRepository, ReadCarImageReadRepository>();
            services.AddScoped<ICarImageFileWriteRepository,CarImageFileWriteRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuthentication, AuthService>();
            services.AddScoped<IInternalAuthentication, AuthService>();

        }
    }
}
