using BulkPaymentServer.Domain.Entities;

namespace BulkPaymentServer.Application.Interfaces;

public interface ICsvParser
{
    List<Payment> Parse(string csvContent);
}