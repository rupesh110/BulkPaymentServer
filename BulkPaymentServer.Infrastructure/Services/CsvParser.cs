using System.IO;
using System.Text.Json;

using BulkPaymentServer.Domain.Entities;
using BulkPaymentServer.Application.Interfaces;

namespace BulkPaymentServer.Infrastructure.Services;

public class CsvParser : ICsvParser
{
    public List<Payment> Parse(string csvStream)
    {
        var payments = new List<Payment>();
        using var reader = new StringReader(csvStream);

        // Read and skip header
        string? headerLine = reader.ReadLine();

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var rows = line.Split(',');

            if (rows.Length < 6)
                continue;

            // Parse CSV values
            int invoiceId = int.Parse(rows[0]);
            string recipientAccount = rows[3];
            string currency = rows[4];
            decimal amount = decimal.Parse(rows[5]);

            var payment = new Payment(
                invoiceId,
                recipientAccount,
                currency,
                amount
            );

            payments.Add(payment);
        }
        Console.WriteLine("CSV parsing completed.");
        Console.WriteLine(JsonSerializer.Serialize(payments));

        return payments;
    }
}
