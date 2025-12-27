using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BulkPaymentServer.Application.Interfaces;

namespace BulkPaymentServer.Infrastructure.Services;

public class AzureBlobStorageService : IFileStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private const string ContainerName = "user-payment-csv";

    public AzureBlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFileAsync(string userId, Stream fileStream, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

        string blobName = $"{userId}/{DateTime.UtcNow:yyyyMMddHHmmss}_{fileName}";

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(fileStream, overwrite: true);

        return blobClient.Uri.ToString();
    }
}