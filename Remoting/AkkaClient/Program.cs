using Akka.Actor;
using Commons;
using Commons.Messages;
using Microsoft.Extensions.Configuration;

namespace AkkaClient1
{
    internal class Program
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
            using var actorSystem = ActorSystem.Create("TestAkktorsClient", akkaConfig);
            var actor = actorSystem.ActorSelection("akka.tcp://TestAkktors@localhost:8081/user/TryTaste"); // remoting

            Thread.Sleep(5000);

            var task = actor.Ask(new Greet("Anonymous5555))"));
            task.Wait();

            Console.WriteLine($"RESULT: {task.Result}");

            var actor1 = actorSystem.ActorSelection("akka.tcp://TestAkktors@localhost:8081/user/TryTaste");
            var task3 = actor1.Ask(new Greet("Anonymous2222))"));
            task3.Wait();

            var task4 = actor1.Ask("EFWERFRRFREFERFREF");
            task4.Wait();

            Console.WriteLine($"RESULTS: {task3.Result}, {task4.Result}");

            Console.ReadKey();
        }
    }
}