using BulkPaymentServer.Application.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace BulkPaymentServer.Infrastructure.Kafka;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    public KafkaProducer(IConfiguration configuration)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],

            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,

            SaslUsername = configuration["Kafka:SaslUsername"],
            SaslPassword = configuration["Kafka:SaslPassword"],

            SslEndpointIdentificationAlgorithm = SslEndpointIdentificationAlgorithm.Https,
            EnableSslCertificateVerification = true,
            Acks = Acks.All
        };

        _topic = configuration["Kafka:TestTopic"]
            ?? throw new InvalidOperationException("Kafka:TestTopic is not configured");

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendMessageAsync(string key, string value)
    {
        await _producer.ProduceAsync(
            _topic,
            new Message<string, string>
            {
                Key = key,
                Value = value
            }
        );
    }
}
