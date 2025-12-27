namespace BulkPaymentServer.Application.DTOs;

public class PaymentResultDto
{
    public string InvoiceNumber { get; set; }
    public string RecipientName { get; set; }
    public string RecipientBsb { get; set; }
    public string RecipientAccount { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
}

