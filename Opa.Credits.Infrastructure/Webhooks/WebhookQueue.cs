using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Opa.Credits.Application.DTOs;
using Opa.Credits.Application.Interfaces;

namespace Opa.Credits.Infrastructure.Webhooks;

public class WebhookQueue : IWebhookQueue
{
    private readonly Channel<WebhookMessage> _queue;

    public WebhookQueue()
    {
        // Usamos una cola Bounded para limitar la RAM en caso de que colapse el sistema externo.
        // Capacidad de 1000 mensajes encolados.
        var options = new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<WebhookMessage>(options);
    }

    public async ValueTask EnqueueAsync(WebhookMessage message)
    {
        await _queue.Writer.WriteAsync(message);
    }

    public async ValueTask<WebhookMessage> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}
