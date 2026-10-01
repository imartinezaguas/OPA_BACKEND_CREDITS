using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace Opa.Credits.Api.Extensions;

public static class ScalarConfigurationExtensions
{
    public static IServiceCollection AddScalarConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Ingresa aquí el token que te devuelve el Login."
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes.Add("Bearer", securityScheme);

                document.SecurityRequirements.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        System.Array.Empty<string>()
                    }
                });

                return System.Threading.Tasks.Task.CompletedTask;
            });
        });
        return services;
    }

    public static WebApplication UseScalarConfiguration(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options => 
            {
                options.WithTitle("OPA API Créditos")
                       .WithTheme(ScalarTheme.Moon);
            });
        }
        
        return app;
    }
}
