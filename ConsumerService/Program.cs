using ConsumerService;
using ConsumerService.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<MessageBusSettings>(builder.Configuration.GetSection("MessageBusSettings"));
builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());
var redis = ConnectionMultiplexer.Connect("your-connection-string");
builder.Services.AddSingleton(redis);
builder.Services.AddSingleton<RedisIdempotencyChecker>();

var app = builder.Build();


app.Run();
