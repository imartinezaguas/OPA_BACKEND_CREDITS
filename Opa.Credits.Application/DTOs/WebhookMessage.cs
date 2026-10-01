using System;

namespace Opa.Credits.Application.DTOs;

public class WebhookMessage
{
    public string Event { get; set; }
    public string EventId { get; set; } = Guid.NewGuid().ToString();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public object Data { get; set; }
}
