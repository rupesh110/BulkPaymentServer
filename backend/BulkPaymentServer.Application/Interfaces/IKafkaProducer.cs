namespace BulkPaymentServer.Application.Interfaces;

public interface IKafkaProducer
{
    Task SendMessageAsync(string key, string message);
}
