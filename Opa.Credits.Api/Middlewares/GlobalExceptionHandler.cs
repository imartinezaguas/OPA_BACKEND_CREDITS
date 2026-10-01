using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Opa.Credits.Domain.Exceptions;

namespace Opa.Credits.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ocurrió un error en la API: {Message}", exception.Message);

        httpContext.Response.ContentType = "application/json";

        object errorResponse;

        switch (exception)
        {
            case NotFoundException notFoundEx:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                errorResponse = new { success = false, error = new { code = "NOT_FOUND", message = notFoundEx.Message } };
                break;
            
            case UnauthorizedAccessException authEx:
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                errorResponse = new { success = false, error = new { code = "UNAUTHORIZED", message = authEx.Message } };
                break;
            
            case BusinessRuleException businessEx:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                errorResponse = new { success = false, error = new { code = "BUSINESS_ERROR", message = businessEx.Message } };
                break;
            
            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                // Ocultar detalles técnicos al user final en errores 500 por seguridad
                errorResponse = new { success = false, error = new { code = "SERVER_ERROR", message = "Ha ocurrido un error inesperado en el servidor." } };
                break;
        }

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        // Retornar true indica que la excepción ya fue manejada y no debe seguir propagándose
        return true; 
    }
}
