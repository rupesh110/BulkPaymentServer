using System.Text.Json;
using BulkPaymentServer.Application.DTOs;
using BulkPaymentServer.Domain.Entities;
using BulkPaymentServer.Application.Interfaces;

namespace BulkPaymentServer.Application.Services;

public class  UploadEventPublisher
{
    private readonly IKafkaProducer _kafkaProducer;

    public UploadEventPublisher(IKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task PublishUploadCreateAsync(Guid uploadId, Payment payment)
    {
        var paymentDto = new KafkaPaymentDto
        {
            InvoiceNumber = payment.InvoiceNumber,
            RecipientName = payment.RecipientName,
            RecipientBsb = payment.RecipientBsb,
            RecipientAccount = payment.RecipientAccount,
            Currency = payment.Currency,
            Amount = payment.Amount
        };

        var kafkaUploadData = new KafkaUploadEvent
        {
            UploadId = uploadId,
            EventType = "PaymentCreated",
            Payload = JsonSerializer.Serialize(paymentDto)
        };

        var messageJson = JsonSerializer.Serialize(kafkaUploadData);
        await _kafkaProducer.SendMessageAsync(uploadId.ToString(), messageJson);
    }

}