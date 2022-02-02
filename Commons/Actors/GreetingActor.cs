using Akka.Actor;
using Commons.Services;

namespace Commons.Messages;

/// <summary>
/// Класс Актора
/// </summary>
public class GreetingActor : ReceiveActor
{
    public Guid NodeId { get; private set; }

    public GreetingActor(DiTest diTest)
    {
        NodeId = Guid.NewGuid();
        Receive<Greet>(greet => Console.WriteLine($"Привет, {greet.Who} {NodeId}, {diTest.Test()}!"));
    }
}