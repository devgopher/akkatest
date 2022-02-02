using Akka.Actor;
using Commons.Messages;
using Commons.Services;
using Hey = Commons.Messages.Hey;

namespace AkkaServer1.Actors;

/// <summary>
/// Класс Актора
/// </summary>
public class TryTasteActor : ReceiveActor
{
    private Guid NodeId { get; set; }
    private List<IActorRef> Actors { get; } = new();
    private readonly DiTest _diTest;


    public TryTasteActor(DiTest diTest)
    {
        _diTest = diTest;
        Init(Sender);
    }

    public void AddClient() => Actors.Add(Sender);

    private void Init(IActorRef sourceActor)
    {
        NodeId = Guid.NewGuid();

        Actors.Clear();

        Receive<Greet>(greet =>
        {
            AddClient();
            var tasteActor = Context.ActorOf<TasteActor>();
            var taste = tasteActor.Ask<Hey>(new Hey());
            taste.Wait();

            foreach (var actor in Actors.Where(actor => actor.GetType() != typeof(DeadLetterActorRef)))
                actor?.Tell($"Привет, {greet.Who} {NodeId}. Попробуй: {taste.Result.Thing}! {_diTest.Test()}", Self);
        });

        ReceiveAny(m =>
        {
            AddClient();
            foreach (var actor in Actors.Where(actor => actor.GetType() != typeof(DeadLetterActorRef)))
                actor?.Tell("Привет, незнакомец!", Self);
        });
    }
}