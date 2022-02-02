/// <summary>
/// Тут будем через kafka сообщения получать и выдавать в ответ что то из БД/массива
/// </summary>

using AkkaDatabaseBack.Handlers;
using Commons;
using Messages;
using Transport.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var cfgPath = "appconfig1.json";
if (args.Length > 0 && File.Exists(args[0]))
    cfgPath = args[0];

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile(cfgPath)
    .Build();


var akkaConfig = config.GetSection("AkkaConfiguration").ToAkkaConfiguration();


//builder.Services.AddKafkaProducer<AskFindVegetableData>();
//builder.Services.AddKafkaConsumer<AskFindVegetableData, VegHandler>(new VegHandler());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");


app.Run();