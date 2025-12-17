using Microsoft.Extensions.Logging;

using BulkPaymentServer.Application.Interfaces;
using BulkPaymentServer.Application.DTOs;
using BulkPaymentServer.Domain.Entities;
using BulkPaymentServer.Application.Services;

namespace BulkPaymentServer.Application.Services;

public class UploadService : IUploadService
{
    private readonly IFileStorageService _fileStorage;
    private readonly ILogger<UploadService> _logger;
    private readonly ICsvProcessor _csvProcessor;
    private readonly IPaymentRepository _paymentRepo;
    private readonly UploadEventPublisher _uploadEventPublisher;

    public UploadService(
        IFileStorageService fileStorage,
        ILogger<UploadService> logger,
        ICsvProcessor csvProcessor,
        IPaymentRepository paymentRepo,
        UploadEventPublisher uploadEventPublisher)
    {
        _fileStorage = fileStorage;
        _logger = logger;
        _csvProcessor = csvProcessor;
        _paymentRepo = paymentRepo;
        _uploadEventPublisher = uploadEventPublisher;
    }

    public async Task<UploadResultDto> UploadFileAsync(string userId, Stream fileStream, string fileName)
    {
        // 1. Upload file to Blob Storage
        var blobUrl = await _fileStorage.UploadFileAsync(userId, fileStream, fileName);

        // 2. Create Upload record
        var upload = new Upload(userId, fileName, blobUrl);
        await _paymentRepo.SaveUploadAsync(upload);

        _logger.LogInformation("File uploaded. Upload record saved with id {UploadId}", upload.Id); //TODO: REMOVE

        // 3. Read CSV
        fileStream.Position = 0;
        using var reader = new StreamReader(fileStream);
        var csvText = await reader.ReadToEndAsync();

        var parsedPayments = await _csvProcessor.ProcessCsvAsync(csvText);

        // 4. Map to domain Payment entities
        var paymentEntities = parsedPayments.Select(p =>
        {
            var payment = new Payment(
                p.InvoiceNumber,
                p.RecipientName,
                p.RecipientBsb,
                p.RecipientAccount,
                p.Currency,
                p.Amount);

            payment.SetUpload(upload.Id);
            return payment;
        }).ToList();

        // 5. Save all payments
        await _paymentRepo.AddPaymentsAsync(paymentEntities);

        // 6. Publish event for further processing
        var publishTasks = paymentEntities
            .Select(payment => _uploadEventPublisher.PublishUploadCreateAsync(upload.Id, payment))
            .ToList();

        await Task.WhenAll(publishTasks);


        return new UploadResultDto(blobUrl, "File uploaded successfully")
        {
            UploadId = upload.Id,
            TotalPayments = paymentEntities.Count
        };
    }
}
