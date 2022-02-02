using Akka.Actor;
using Commons.Messages;

namespace Commons.Actors
{
    // Runs in a separate process from SendActor
    public class EchoActor : ReceiveActor
    {
        public EchoActor() => Receive<Hey>(msg => Sender.Tell(msg));
    }
}
