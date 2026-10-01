using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Opa.Credits.Api.Extensions;

public static class CorsConfigurationExtensions
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
        
        return services;
    }

    public static WebApplication UseCorsConfiguration(this WebApplication app)
    {
        app.UseCors("AllowAngular");
        return app;
    }
}
