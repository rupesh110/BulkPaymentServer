namespace BulkPaymentServer.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public int InvoiceId { get; private set; }
    public string RecipientAccount { get; private set; } = string.Empty;
    public string Currency { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Status { get; private set; } = "Pending";

    public Payment(int invoiceId, string recipientAccount, string currency, decimal amount)
    {
        if (invoiceId <= 0) 
        {
            throw new ArgumentException("InvoiceId must be positive Integer.", nameof(invoiceId));
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
        InvoiceId = invoiceId;
        RecipientAccount = recipientAccount;
        Currency = currency;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
        Status = "Pending";
    }

    private Payment() { }
}
