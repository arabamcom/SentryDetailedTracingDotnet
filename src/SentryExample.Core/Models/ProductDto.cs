using SentryExample.Core.Kafka.Events;

namespace SentryExample.Core.Models
{
    public class ProductDto : IEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public decimal Rate { get; set; }

        public IEventTopicType Topic => IEventTopicType.AddWeatherForecastV1;
    }
}
