using BulkPaymentServer.Domain.Entities;

namespace BulkPaymentServer.Application.Interfaces;

public interface IPaymentRepository
{
    Task AddPaymentsAsync(IEnumerable<Payment> payments);
    Task SaveUploadAsync(Upload upload);
}
