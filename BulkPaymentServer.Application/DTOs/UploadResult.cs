namespace BulkPaymentServer.Application.DTOs;

public class UploadResult
{
    public string BlobUrl { get; }
    public string Message { get; }

    public UploadResult(string blobUrl, string message)
    {
        BlobUrl = blobUrl;
        Message = message;
    }
}
