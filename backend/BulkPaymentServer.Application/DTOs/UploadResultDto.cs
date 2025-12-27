namespace BulkPaymentServer.Application.DTOs;

public class UploadResultDto
{
    public Guid UploadId { get; set; }
    public string BlobUrl { get; }
    public int TotalPayments { get; set; }
    public string Message { get; }

    public UploadResultDto(string blobUrl, string message)
    {
        BlobUrl = blobUrl;
        Message = message;
    }
}
