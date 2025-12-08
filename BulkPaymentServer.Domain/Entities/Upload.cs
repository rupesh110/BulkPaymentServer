
namespace BulkPaymentServer.Domain.Entities;

public class Upload
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public string FileName { get; private set; } = string.Empty;
    public string BlobUrl { get; private set; } = string.Empty;
    public DateTime UploadedAt { get; private set; }

    public List<Payment> Payments { get; private set; } = new();

    public Upload(string userId, string fileName, string blobUrl)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("FileName is required.", nameof(fileName));
        }

        if (string.IsNullOrWhiteSpace(blobUrl))
        {
            throw new ArgumentException("BlobUrl is required.", nameof(blobUrl));
        }

        Id = Guid.NewGuid();
        UserId = userId;
        FileName = fileName;
        BlobUrl = blobUrl;
        UploadedAt = DateTime.UtcNow;
    }

    private Upload() { }

    public void AddPayment(Payment payment)
    {
        if (payment == null)
        {
            throw new ArgumentNullException(nameof(payment));
        }
        Payments.Add(payment);
    }

}