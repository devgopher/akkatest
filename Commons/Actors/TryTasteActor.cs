using Akka.Actor;
using Commons.Actors;
using Commons.Messages;
using Commons.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AkkaWebApp.Actors;

/// <summary>
/// Класс Актора
/// </summary>
public class TryTasteActor : ReceiveActor
{
    private Guid NodeId { get; }
    public TryTasteActor(IServiceProvider sp)
    {
        NodeId = Guid.NewGuid(); 

        Receive<Greet>(greet =>
        {
            var diTest = sp.GetRequiredService<DiTest>();
            var tasteActor = Context.ActorOf<TasteActor>();
            var taste = tasteActor.Ask<Hey>(new Hey());
            taste.Wait();
            Sender.Tell($"Привет, {greet.Who} {NodeId}. Попробуй: {taste.Result.Thing} {diTest.Test()}!");
        });

        ReceiveAny( m => Sender.Tell($"Привет, незнакомец!"));
    }
}