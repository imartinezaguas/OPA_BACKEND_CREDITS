using Microsoft.EntityFrameworkCore;
using Opa.Credits.Infrastructure.Persistence;
using Opa.Credits.Application.Interfaces;
using Opa.Credits.Infrastructure.Repositories;
using Opa.Credits.Application.Services;
using FluentValidation;
using Opa.Credits.Application.Validations;
using Opa.Credits.Application.DTOs;
using Opa.Credits.Api.Extensions;
using Opa.Credits.Api.Middlewares;
using Opa.Credits.Infrastructure.Services;
using Opa.Credits.Infrastructure.Webhooks;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuración separada por principio de Responsabilidad Única (SOLID)
builder.Services.AddCorsConfiguration();
builder.Services.AddJwtConfiguration(builder.Configuration);

// Inyección de Dependencias (Arquitectura Limpia)
builder.Services.AddScoped<IAssociateRepository, AssociateRepository>();
builder.Services.AddScoped<ICreditRepository, CreditRepository>();
builder.Services.AddScoped<ICreditService, CreditService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Servicios de Infraestructura Asíncrona (Webhooks)
builder.Services.AddSingleton<IWebhookQueue, WebhookQueue>();
builder.Services.AddHostedService<WebhookDispatcherService>();
builder.Services.AddHttpClient(); // Necesario para disparar las peticiones externas

// Registrar Validadores de FluentValidation explícitamente para evitar ReflectionTypeLoadException
builder.Services.AddScoped<IValidator<CreateCreditDto>, CreateCreditValidator>();

// Configuración de PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseLowerCaseNamingConvention());

// Manejo Global de Excepciones (.NET 8+)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Configuración oculta de la documentación (Scalar API) mediante método de extensión
builder.Services.AddScalarConfiguration();

var app = builder.Build();

// Ejecutar migraciones automáticamente al arrancar el contenedor
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    
    // Crear un Asociado de prueba si la tabla está vacía para poder operar
    if (!dbContext.Associates.Any())
    {
        dbContext.Associates.Add(new Opa.Credits.Domain.Entities.Associate { Identification = "123456789", Name = "Asociado de Prueba" });
        dbContext.SaveChanges();
    }
}

// Pipeline de Scalar oculto
app.UseScalarConfiguration(app.Environment);

app.UseCorsConfiguration(); // CORS DEBE ir antes del ExceptionHandler para no bloquear los errores JSON
app.UseExceptionHandler(); // Red de seguridad global para atrapar excepciones

app.UseHttpsRedirection();

// Pipeline segmentado
app.UseJwtConfiguration();

app.MapControllers();

app.Run();
