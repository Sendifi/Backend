using Microsoft.EntityFrameworkCore;

// Shared
using WebApplication2.Shared.Domain.Repositories;
using WebApplication2.Shared.Infrastructure.Interfaces.ASP.Configuration;
using WebApplication2.Shared.Infrastructure.Persistence.EFC.Configuration;
using WebApplication2.Shared.Infrastructure.Persistence.EFC.Repositories;

// Courier BC
using WebApplication2.Courier.Domain.Repositories;
using WebApplication2.Courier.Domain.Services;
using WebApplication2.Courier.Infrastructure.Repositories;
using WebApplication2.Courier.Application.Internal.CommandServices;
using WebApplication2.Courier.Application.Internal.QueryServices;

var builder = WebApplication.CreateBuilder(args);

// ---------- MVC & Swagger ----------
builder.Services.AddRouting(o => o.LowercaseUrls = true);
builder.Services.AddControllers(o => o.Conventions.Add(new KebabCaseRouteNamingConvention()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.EnableAnnotations());

// ---------- DbContext (MySQL) ----------
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var cs = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(cs))
            throw new Exception("Database connection string is not set (DefaultConnection).");

        options.UseMySQL(cs)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var csTemplate = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(csTemplate))
            throw new Exception("Database connection string template is not set.");
        var cs = Environment.ExpandEnvironmentVariables(csTemplate);

        options.UseMySQL(cs)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
    });
}

// ---------- DI ----------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<ICourierCommandService, CourierCommandService>();
builder.Services.AddScoped<ICourierQueryService, CourierQueryService>();

var app = builder.Build();

// ---------- Ensure DB objects exist ----------
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Crea la BD y tablas desde el modelo si no existen
    ctx.Database.EnsureCreated();
    // Si luego agregas Migraciones, puedes cambiar a:
    // ctx.Database.Migrate();
}

// ---------- HTTP pipeline ----------
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
