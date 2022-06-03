using System.Text.Json.Serialization;

namespace SentryExample.Core.Kafka.Events
{
    public interface IEvent
    {
        [JsonIgnore]
        IEventTopicType Topic { get; }
    }
}
