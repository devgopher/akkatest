using System.Collections.Concurrent;
using Akka.Actor;
using AkkaServer1.Actors;
using MassTransit;
using Messages;

namespace AkkaServer1;

public class KafkaMessageConsumer : IConsumer<HelloMessage>
{
    private readonly ActorSystem _actorSystem;
    private readonly ConcurrentDictionary<string, IActorRef> _actors = new();

    public KafkaMessageConsumer(ActorSystem actorSystem)
        => _actorSystem = actorSystem;

    public async Task Consume(ConsumeContext<HelloMessage> context)
    {
        var actor = _actors.Values.FirstOrDefault(x => !((HelloActor)x).isBusy);
        if (actor == default) 
            actor = _actorSystem.ActorOf(Props.Create(() => new HelloActor()));

        actor.Tell(context.Message);
    }
}