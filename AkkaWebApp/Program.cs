using Akka.Actor;
using Akka.DependencyInjection;
using AkkaWebApp;
using Commons.Services;
using MassTransit;
using MassTransit.KafkaIntegration;
using Messages;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "akka test", Version = "v1" }));

var cfgPath = "appconfig1.json";
if (args.Any() && File.Exists(args[0]))
    cfgPath = args[0];

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile(cfgPath)
    .Build();

var akkaConfig = config.GetSection("AkkaConfiguration").ToAkkaConfiguration();

// Инжектим сервисы. коотрые будут в акторах

builder.Services.AddTransient<DiTest>();

// создаем объект типа DepInjectionResolversetup

var di = DependencyResolverSetup.Create(builder.Services.BuildServiceProvider());

// Создаем систему акторов c DI
var actorsSetup = BootstrapSetup.Create()
    .WithConfig(akkaConfig)
    .And(di);

var actorSystem = ActorSystem.Create("TestAkktors", actorsSetup);

builder.Logging.AddConsole();
builder.Logging.AddEventLog();

builder.Services.AddSingleton(actorSystem);
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

    x.AddRider(rider =>
    {
        rider.AddProducer<Guid, HelloMessage>(nameof(HelloMessage));

        rider.UsingKafka((context, k) => k.Host("localhost:9092"));
    });
});

builder.Services.AddMassTransitHostedService();

//builder.Services.AddScoped<IGreetingActor, TryTasteActor>(a => (IGreetingActor)actorSystem.ActorOf<TryTasteActor>())

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnionMessengerClient1 v1"));
}

app.MapControllers();
app.Run();