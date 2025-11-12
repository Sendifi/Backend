using Delivery.Api.DeliveryPersons.Domain.Repositories;
using Delivery.Api.DeliveryPersons.Domain.Services;
using Delivery.Api.DeliveryPersons.Infrastructure.Repositories;
using Delivery.Api.DeliveryPersons.Infrastructure.Services;
using Delivery.Api.Shared.Persistence;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

// 🔹 Swagger: registrar SIEMPRE
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (ajusta el connection string si hace falta)
builder.Services.AddDbContext<DeliveryDbContext>(options =>
    options.UseInMemoryDatabase("DeliveryDb")); // o UseSqlServer(...)

// Repositorios
builder.Services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Servicios de dominio
builder.Services.AddScoped<IDeliveryPersonCommandService, DeliveryPersonCommandService>();
builder.Services.AddScoped<IDeliveryPersonQueryService, DeliveryPersonQueryService>();

var app = builder.Build();

// 🔹 Middleware Swagger SIEMPRE
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery API v1");
});

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Endpoint raíz
app.MapGet("/", () => Results.Ok("Delivery API corriendo ✅"));

app.Run();