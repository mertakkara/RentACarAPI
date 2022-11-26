using FluentValidation.AspNetCore;
using RentACarAPI.Application.Validators.Cars;
using RentACarAPI.Infrastructure;
using RentACarAPI.Infrastructure.Filters;
using RentACarAPI.Infrastructure.Services.Storage.Azure;
using RentACarAPI.Infrastructure.Services.Storage.Local;
using RentACarAPI.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
//builder.Services.AddStorage(StorageType.Azure);
//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
  policy.WithOrigins("http://localhost:4200","https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
));
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateCarValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
