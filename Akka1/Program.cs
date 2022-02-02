using Akka.Actor;
using Commons;
using Commons.Messages;
using Microsoft.Extensions.Configuration;

namespace Akka1
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfgPath = "appconfig1.json";
            if (args.Length > 0 && File.Exists(args[0]))
                cfgPath = args[0];

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(cfgPath).Build();


            var akkaConfig = config.GetSection("AkkaConfiguration").ToAkkaConfiguration();

            var actorSystem = ActorSystem.Create("TestAkktors", akkaConfig);
            var actor = actorSystem.ActorOf<GreetingActor>("MyAkktor");

            Console.WriteLine("Введите имя: ");

            var name = Console.ReadLine();

            actor.Tell(new Greet(name));

            actor.Tell(new Greet(name));

            actor.Tell(new Greet(name));

            actorSystem.RegisterOnTermination(() => Console.WriteLine("Amazing termination))))"));

            Console.ReadKey();
        }
    }
}