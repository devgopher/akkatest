using Confluent.Kafka;
using Messages.Serialization;
using Polly;

namespace Transport.Kafka
{
    internal class KafkaConsumer<TMsg> : IConsumer<TMsg>, IDisposable
        where TMsg : class
    {
        private IConsumer<Ignore, TMsg> _consumer;

        public KafkaConsumer() => Init();

        public void Init()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true
            };

            _consumer = new ConsumerBuilder<Ignore, TMsg>(config)
                .SetValueDeserializer(new JsonMessageDeserializer<TMsg>())
                .Build();
        }

        public void Subscribe<THndl>(THndl? handler, CancellationToken token)
            where THndl : class, IHandler<TMsg>
        {
            _consumer.Subscribe(new string[] { typeof(TMsg).Name });

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var retryPolicy = Policy
                        .HandleResult<bool>(res => res == false)
                        .WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(1));

                    retryPolicy.ExecuteAsync(async () => await InnerConsume(handler));

                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task<bool> InnerConsume<THndl>(THndl? handler) where THndl : class, IHandler<TMsg>
        {
            var consumeResult = _consumer.Consume(5000);
            if (consumeResult == default)
                return false;

            
            var consumedMessage = consumeResult.Message?.Value;

            if (handler != default && consumedMessage != default)
                await handler.Handle(consumedMessage);

            return true;
        }

        public void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
        }
    }
}
