using Akka.Actor;
using AkkaServer1;
using AkkaServer1.Actors;
using Commons;
using Commons.Services;
using MassTransit;
using Messages;

var cfgPath = "appconfig1.json";
if (args.Length > 0 && File.Exists(args[0]))
    cfgPath = args[0];

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile(cfgPath).Build();


var akkaConfig = config
    .GetSection("AkkaConfiguration")
    .ToAkkaConfiguration();

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Logging.AddEventLog();

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

    x.AddRider(rider =>
    {
        rider.AddConsumer<KafkaMessageConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host("localhost:9092");
            k.TopicEndpoint<Guid, HelloMessage>(nameof(HelloMessage),
                "foo",
                e =>
                {
                    e.ConfigureConsumer<KafkaMessageConsumer>(context);
                    e.CreateIfMissing();
                });
        });
    });
});

builder.Services.AddMassTransitHostedService(true);

var actorSystem = ActorSystem.Create("TestAkktors", akkaConfig);

builder.Services.AddScoped(x => new KafkaMessageConsumer(actorSystem));
builder.Services.AddSingleton(actorSystem);
actorSystem.ActorOf(Props.Create(() => new TryTasteActor(new DiTest())), "TryTaste");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.Run();