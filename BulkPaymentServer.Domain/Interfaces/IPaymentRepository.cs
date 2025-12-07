using BulkPaymentServer.Domain.Entities;

namespace BulkPaymentServer.Domain.Interfaces;

public interface IPaymentRepository
{
    Task AddPaymentAsync(Payment payment);
    Task AddRangeAsync(IEnumerable<Payment> payments);
    Task SaveChangesAsync();
}