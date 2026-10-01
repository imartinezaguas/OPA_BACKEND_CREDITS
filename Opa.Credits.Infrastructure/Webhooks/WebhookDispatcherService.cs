using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opa.Credits.Application.Interfaces;

namespace Opa.Credits.Infrastructure.Webhooks;

public class WebhookDispatcherService : BackgroundService
{
    private readonly IWebhookQueue _queue;
    private readonly ILogger<WebhookDispatcherService> _logger;

    public WebhookDispatcherService(IWebhookQueue queue, ILogger<WebhookDispatcherService> logger)
    {
        _queue = queue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de Webhooks iniciado. Esperando eventos para registrar...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // El hilo se suspende amigablemente hasta que caiga un nuevo mensaje en la cola
                var message = await _queue.DequeueAsync(stoppingToken);
                
                // Serializamos el payload exacto que pide el PDF
                string jsonPayload = JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true });
                
                // Guardamos la traza (En consola) simulando el envío
                _logger.LogInformation("=== [TRAZA DE WEBHOOK ENVIADO] ===");
                _logger.LogInformation("Notificando a sistema externo el evento: {Event}", message.Event);
                _logger.LogInformation("Payload:\n{Payload}", jsonPayload);
                _logger.LogInformation("==================================");
            }
            catch (OperationCanceledException)
            {
                break; // Se apagó la API
            }
            catch (Exception ex)
            {
                // Atrapamos el error para que NO se muera el BackgroundService
                _logger.LogError(ex, "Error procesando la traza del webhook.");
            }
        }
    }
}
