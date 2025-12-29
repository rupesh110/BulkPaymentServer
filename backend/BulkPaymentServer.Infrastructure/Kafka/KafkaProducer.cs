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
            Acks = Acks.All,

            //default for local
            SecurityProtocol = SecurityProtocol.Plaintext
        };

        var saslUsername = configuration["Kafka:SaslUsername"];
        var saslPassword = configuration["Kafka:SaslPassword"];

        // enable SASL only when creds exist
        if (!string.IsNullOrWhiteSpace(saslUsername))
        {
            config.SecurityProtocol = SecurityProtocol.SaslSsl;
            config.SaslMechanism = SaslMechanism.Plain;
            config.SaslUsername = saslUsername;
            config.SaslPassword = saslPassword;

            config.SslEndpointIdentificationAlgorithm =
                SslEndpointIdentificationAlgorithm.Https;
            config.EnableSslCertificateVerification = true;
        }

        _topic = configuration["Kafka:PaymentTopic"]
            ?? throw new InvalidOperationException("Kafka:PaymentTopic is not configured");

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
