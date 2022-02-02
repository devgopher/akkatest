using Akka.Actor;
using Commons.Messages;

namespace AkkaServer1.Actors;

public class TasteActor : ReceiveActor
{
    private readonly List<string> _variants = new()
    {
        "Пирог",
        "Торт",
        "Груши",
        "Яблоки",
        "Картошку",
        "Орехи"
    };

    public TasteActor()
    {
        var rand = new Random(DateTime.Now.Millisecond);
        Receive<Hey>(greet => Sender.Tell(new Hey
        {
            Thing = _variants[rand.Next() % _variants.Count]
        }));

        ReceiveAny(greet => Sender.Tell("Ты не проявил уважения... Где твое \"здрасьте\"?"));
    }
}