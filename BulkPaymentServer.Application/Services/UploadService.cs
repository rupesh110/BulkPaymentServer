using Microsoft.Extensions.Logging;


using BulkPaymentServer.Application.Interfaces;
using BulkPaymentServer.Application.DTOs;

namespace BulkPaymentServer.Application.Services;

public class UploadService : IUploadService
{
    private readonly IFileStorageService _fileStorage;
    private readonly ILogger<UploadService> _logger;

    public UploadService(
        IFileStorageService fileStorage,
        ILogger<UploadService> logger)
    {
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<UploadResult> UploadFileAsync(string userId, Stream fileStream, string fileName)
    {
        var blobUrl = await _fileStorage.UploadFileAsync(userId, fileStream, fileName);

        return new UploadResult(blobUrl, "File uploaded successfully");
    }
}
