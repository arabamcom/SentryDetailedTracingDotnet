using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SentryExample.Core.Helpers;
using SentryExample.Core.Kafka.Events;
using SentryExample.Core.Models;
using System.Net;

namespace SentryExample.Core.Kafka
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService (IOptions<KafkaOptions> kafkaOptions,
            ILogger<KafkaProducerService> logger)
        {
            _logger = logger;
            var conf = new ProducerConfig
            {
                BootstrapServers = kafkaOptions.Value.Servers,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslPlaintext,
                SaslUsername = kafkaOptions.Value.Credentials.Username,
                SaslPassword = kafkaOptions.Value.Credentials.Password,
                EnableDeliveryReports = false,
                SocketTimeoutMs = kafkaOptions.Value.Producer.SocketTimeoutMs,
                MessageTimeoutMs = kafkaOptions.Value.Producer.MessageTimeoutMs
            };
            _producer = new ProducerBuilder<Null, string>(conf).SetErrorHandler((_producer, error) =>
            {
                if (error.IsError || error.IsLocalError || error.IsBrokerError || error.IsFatal)
                {
                    _logger.LogError($"Kafka producer exception occured Reason: {error.Reason} Code: {error.Code}");
                }
            }).Build();
        }

        public IOperationResult Produce(IEvent eventSource, string topic = null)
        {
            if (string.IsNullOrWhiteSpace(topic))
                return new OperationResult(false, message: $"The topic named {topic} was not found.", (int)HttpStatusCode.OK);
            try
            {
                _producer.Produce(topic, new Message<Null, string>
                {
                    Value = JsonHelper.SerializeWithIgnoreRelations(eventSource)
                });

            }
            catch (ProduceException<Null, string> e)
            {
                return new OperationResult(false);
            }
            return new OperationResult(true);
        }

        public async Task<IOperationResult> ProduceAsync(IEvent eventSource, string topic = null)
        {
            if (string.IsNullOrWhiteSpace(topic))
                return new OperationResult(false, message: $"The topic named {topic} was not found.", (int)HttpStatusCode.OK);
            try
            {
                await _producer.ProduceAsync(topic, new Message<Null, string> {
                    Value = JsonHelper.SerializeWithIgnoreRelations(eventSource)
                });
            }
            catch (ProduceException<Null, string> e)
            {
                return new OperationResult(false);
            }
            return new OperationResult(true);
        }

        public IOperationResult Delay()
        {
            Task.Delay(2 * 1000);
            return new OperationResult(true);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
