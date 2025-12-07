using BulkPaymentServer.Domain.Entities;
using BulkPaymentServer.Application.Interfaces;

namespace BulkPaymentServer.Application.Services;

public class CsvProcessor : ICsvProcessor
{
    private readonly ICsvParser _csvParser;
    public CsvProcessor(ICsvParser csvParser)
    {
        _csvParser = csvParser;
    }

    public Task<List<Payment>> ProcessCsvAsync(string csvStream)
    {
        Console.WriteLine("Starting CSV processing...");

        var payments = _csvParser.Parse(csvStream);


        Console.WriteLine("Starting CSV processing...");
        return Task.FromResult(payments);
    }
}