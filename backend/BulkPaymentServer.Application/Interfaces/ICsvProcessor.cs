using BulkPaymentServer.Domain.Entities;

namespace BulkPaymentServer.Application.Interfaces;

public interface ICsvProcessor
{
    Task<List<Payment>> ProcessCsvAsync(string csvStream);
}

