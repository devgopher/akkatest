using Akka.Actor;
using Akka.Streams.Kafka.Settings;
using Messages;

namespace AkkaServer1.Actors;

public class HelloActor : ReceiveActor
{
    private ConsumerSettings<Guid, HelloMessage> _consumerSettings;

    public bool isBusy { get; private set; }

    public HelloActor()
    {
        Receive<HelloMessage>(m =>
        {
            isBusy = true;
            Console.WriteLine($"ECHO: {m.Hello}");
            Thread.Sleep(5000);
            isBusy = false;
        });

        ReceiveAny(m => Console.WriteLine($"ECHO: ANY"));
    }
}