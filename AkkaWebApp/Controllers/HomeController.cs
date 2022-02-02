using Akka.Actor;
using Akka.Streams.Dsl;
using MassTransit;
using MassTransit.KafkaIntegration;
using Messages;
using Microsoft.AspNetCore.Mvc;

namespace AkkaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ActorSystem _actorSystem;
        private ITopicProducer<Guid, HelloMessage> _producer;
        private IBusControl _bus;

        //public IActorRef GetActor()
        //    => _actorSystem.ActorOf(DependencyResolver.For(_actorSystem).Props<TryTasteActor>(),"ttt1");

        public HomeController(ActorSystem actorSystem, ITopicProducer<Guid, HelloMessage> producer , IBusControl bus)
        {
            _actorSystem = actorSystem;
            _producer = producer;
            _bus = bus;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> TestMassTransit()
        {
            var messages = 
                Enumerable.Range(1, 100000)
                .Select(_ => new HelloMessage
                {
                    Hello = "test"
                });

            foreach (var message in messages)
                await _producer.Produce(Guid.NewGuid(), message);

            return Ok();
        }
    }
}