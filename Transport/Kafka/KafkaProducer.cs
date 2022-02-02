using Akka.Streams.Kafka.Settings;
using Confluent.Kafka;
using Messages.Serialization;
using Config = Akka.Configuration.Config;

namespace Transport.Kafka
{
    public class KafkaProducer<TMsg> : IProducer<TMsg>, IDisposable
        where TMsg : class
    {
        private IProducer<Null, TMsg> _producer = default;

        public KafkaProducer() => Init();

        public void Init()
        {
            var config = new Config
            {
                Substitutions = null,
            };

            _producer = ProducerSettings<Null, TMsg>
                .Create(config, null, new JsonMessageSerializer<TMsg>())
                .WithBootstrapServers("localhost:9092")
                .CreateKafkaProducer();
        }

        public async Task<bool> ProduceAsync(TMsg message)
        {
            var result = await _producer.ProduceAsync(typeof(TMsg).Name, new Message<Null, TMsg> { Value = message });
            return result.Status != PersistenceStatus.NotPersisted;
        }

        public void Dispose() =>_producer.Dispose();
    }
}
