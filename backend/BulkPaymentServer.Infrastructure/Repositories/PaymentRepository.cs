using BulkPaymentServer.Application.Interfaces;
using BulkPaymentServer.Domain.Entities;
using BulkPaymentServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BulkPaymentServer.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly BulkPaymentDbContext _db;

    public PaymentRepository(BulkPaymentDbContext db)
    {
        _db = db;
    }

    public async Task SaveUploadAsync(Upload upload)
    {
        _db.Uploads.Add(upload);
        await _db.SaveChangesAsync();
    }

    public async Task AddPaymentsAsync(IEnumerable<Payment> payments)
    {
        _db.Payments.AddRange(payments);
        await _db.SaveChangesAsync();
    }
}
