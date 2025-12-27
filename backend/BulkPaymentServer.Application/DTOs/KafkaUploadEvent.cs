

namespace BulkPaymentServer.Application.DTOs;

public class KafkaUploadEvent
{
    public Guid UploadId { get; set; }
    public string EventType { get; set; }
    public string Payload { get; set; }
    public EventMeta Meta { get; set; } = new();
}

public class EventMeta
{
    public int RetryCount { get; set; }
    public string? LastFailureReason { get; set; }
    public DateTime CreatedAt { get; set; }
}
