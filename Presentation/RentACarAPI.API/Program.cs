using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using RentACarAPI.API.Configurations.ColumnWriters;
using RentACarAPI.Application;
using RentACarAPI.Application.Validators.Cars;
using RentACarAPI.Infrastructure;
using RentACarAPI.Infrastructure.Filters;
using RentACarAPI.Infrastructure.Services.Storage.Azure;
using RentACarAPI.Infrastructure.Services.Storage.Local;
using RentACarAPI.Persistence;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
//builder.Services.AddStorage(StorageType.Azure);
//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
  policy.WithOrigins("http://localhost:4200","https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
));
Logger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"),"logs",
    needAutoCreateTable: true,// yoksa o tabloyu uygulama ayaða kalakarken oluþturur
    columnOptions: new Dictionary<string, ColumnWriterBase>
    {
        {"message",new RenderedMessageColumnWriter() },
        {"message_template",new MessageTemplateColumnWriter() },
        {"level",new LevelColumnWriter() },
        {"time_stamp",new TimestampColumnWriter() },
        {"exception",new ExceptionColumnWriter() },
        {"log_event",new LogEventSerializedColumnWriter() },
        {"user_name",new UsernameColumnWriter() }

    }).WriteTo.Seq(builder.Configuration["Seq:Server"]).Enrich.FromLogContext()// custom alanlar varsa bu yazýlýr
    .MinimumLevel.Information()

    .CreateLogger();
builder.Host.UseSerilog(logger);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");// kullanýcýya baðlý bütün bilgiler gelmesi için
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});


builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateCarValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin",options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateAudience = true, // Oluþturulan token hangi originlerin kullanýcý belirlediðimi< deðerdir. www.aaaa.com
        ValidateIssuer = true, // kimin daðýttýðý www.myapi.com
        ValidateLifetime = true, // oluþturulan token deðerinin süresi kontrol edecek
        ValidateIssuerSigningKey = true, // Üretilecek token uygulamamýza ait oludðunu eden secret key
        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore,expires,securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow:false,
        NameClaimType = ClaimTypes.Name //JWT üzerinde Name Claim'ine karþýlýk gelen deðer User.Identity.Name Propertinden alýnýr
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();//bundan öncekiler loglanmaz
app.UseHttpLogging();// yapýlan isteklerin loglanmasý için
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();
app.Use(async(context,next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name: null;
    LogContext.PushProperty("user_name",username);
    await next();
});

app.MapControllers();

app.Run();
