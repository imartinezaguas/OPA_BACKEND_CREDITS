using System.Threading;
using System.Threading.Tasks;
using Opa.Credits.Application.DTOs;

namespace Opa.Credits.Application.Interfaces;

public interface IWebhookQueue
{
    ValueTask EnqueueAsync(WebhookMessage message);
    ValueTask<WebhookMessage> DequeueAsync(CancellationToken cancellationToken);
}
