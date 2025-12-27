using System.IO;
using System.Text.Json;
using BulkPaymentServer.Domain.Entities;
using BulkPaymentServer.Application.Interfaces;

namespace BulkPaymentServer.Infrastructure.Services;

public class CsvParser : ICsvParser
{
    public List<Payment> Parse(string csvContent)
    {
        var payments = new List<Payment>();
        using var reader = new StringReader(csvContent);

        // Skip header
        string? headerLine = reader.ReadLine();

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var rows = line.Split(',');

            if (rows.Length < 6)
                continue;

            int invoiceNumber = int.Parse(rows[0]);
            string recipientName = rows[1];
            string recipientBsb = rows[2];
            string recipientAccount = rows[3];
            string currency = rows[4];
            decimal amount = decimal.Parse(rows[5]);

            var payment = new Payment(
                invoiceNumber,
                recipientName,
                recipientBsb,
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
