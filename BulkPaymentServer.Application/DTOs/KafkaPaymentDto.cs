namespace BulkPaymentServer.Application.DTOs;

public class KafkaPaymentDto
{
    public int InvoiceNumber { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientBsb { get; set; } = string.Empty;
    public string RecipientAccount { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
