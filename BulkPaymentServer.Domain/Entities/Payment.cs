namespace BulkPaymentServer.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid UploadId { get; set; }
    public Upload Upload { get; private set; } 
    public int InvoiceNumber { get; private set; }
    public string RecipientName { get; private set; } = string.Empty;
    public string RecipientBsb { get; private set; } = string.Empty;
    public string RecipientAccount { get; private set; } = string.Empty;
    public string Currency { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Status { get; private set; } = "Pending";

    public Payment(
        int invoiceNumber, 
        string recipientName,
        string recipientBsb,
        string recipientAccount, 
        string currency, 
        decimal amount)
    {
        if (invoiceNumber <= 0) 
        {
            throw new ArgumentException("InvoiceId must be positive Integer.", nameof(invoiceNumber));
        }

        if (string.IsNullOrWhiteSpace(recipientName))
        {
            throw new ArgumentException("RecipientName must be provided.", nameof(recipientName));
        }

        if (string.IsNullOrWhiteSpace(recipientAccount))
        {
            throw new ArgumentException("RecipientAccount must be provided.", nameof(recipientAccount));
        }

        if (string.IsNullOrWhiteSpace(currency)) 
        {
            throw new ArgumentException("Currency must be provided.", nameof(currency));
        }

        if (amount <= 0) 
        {
            throw new ArgumentException("Amount must be positive number.", nameof(amount));
        }

        Id = Guid.NewGuid();
        InvoiceNumber = invoiceNumber;
        RecipientName = recipientName;
        RecipientBsb = recipientBsb;
        RecipientAccount = recipientAccount;
        Currency = currency;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
        Status = "Pending";
    }

    public void SetUpload(Guid uploadId)
    {
        UploadId = uploadId;
    }
    private Payment() { }
}
