using SentryExample.Core.Kafka.Events;
using SentryExample.Core.Models;

namespace SentryExample.Core.Kafka
{
    public interface IKafkaProducerService : IDisposable
    {
        IOperationResult Produce(IEvent eventSource, string topic = null);
        Task<IOperationResult> ProduceAsync(IEvent eventSource, string topic = null);
        IOperationResult Delay();
    }
}
