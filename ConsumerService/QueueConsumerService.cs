using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using MediatR;
using System.Text.Json;
using System.Threading.Channels;
using ConsumerService.Caching;

namespace ConsumerService;

public class QueueConsumerService : BackgroundService
{
    private readonly MessageBusSettings _settings;
    private readonly ILogger<QueueConsumerService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Dictionary<string, Type> _messageTypes;
    private IConnection _connection;
    private IModel _channel;
    private readonly RedisIdempotencyChecker _redisChecker;

    public QueueConsumerService(
        IOptions<MessageBusSettings> settings,
        ILogger<QueueConsumerService> logger,
        IServiceScopeFactory serviceScopeFactory,
        Dictionary<string, Type> messageTypes,
        RedisIdempotencyChecker redisIdempotencyChecker
        )
    {
        _settings = settings.Value;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _messageTypes = messageTypes;
        _redisChecker = redisIdempotencyChecker;
        InitRabbitMQ();
    }

    //Create a shared connection and maybe channel if more services are to be added. 
    private void InitRabbitMQ()
    {      
        var factory = new ConnectionFactory { HostName = _settings.Hostname };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _settings.QueueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false
                            );
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) => await ProcessMessage(ea, stoppingToken);

        _channel.BasicConsume(_settings.QueueName, true, consumer);

        await Task.CompletedTask;
    }

    private async Task ProcessMessage(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
    {
        //if we need batch polling instead we could wrap this in a foreach
        if (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Cancellation requested. Stopping message processing.");
            return;
        }
        var deliveryTag = ea.DeliveryTag;
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var messageType = ea.BasicProperties.Type;

        var type = _messageTypes.GetValueOrDefault(messageType);

        if (type == null)
        {
            _logger.LogWarning($"Unknown message type: {messageType}");
            return;
        }

        try
        {
            dynamic typedMessage = JsonSerializer.Deserialize(message, type);

            if (typedMessage == null)
            {
                _logger.LogWarning($"Deserialization failed for message type: {messageType}");
                return;
            }

            if (await _redisChecker.IsOperationAlreadyPerformedAsync(typedMessage.Id.ToString()))
            {
                _logger.LogInformation($"Message {typedMessage.Id} already processed. Skipping.");
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(typedMessage as IRequest, stoppingToken);
            await _redisChecker.MarkOperationAsPerformedAsync(typedMessage.Id.ToString());

            _channel.BasicAck(deliveryTag, false);
        }
        catch (Exception ex)
        {
            HandleRetry(message, messageType, type, ex);
            _channel.BasicAck(deliveryTag, false);
        }
    }

    private void HandleRetry(string message, string messageType, Type type, Exception ex)
    {
        dynamic typedMessage = JsonSerializer.Deserialize(message, type);

        typedMessage.RetryCount = (typedMessage.RetryCount ?? 0) + 1;

        if (typedMessage.RetryCount > 5)
        {
            _logger.LogError(ex, "Max retry attempts reached. Moving to error queue.");
        }
        else
        {
            var properties = _channel.CreateBasicProperties();
            properties.Type = messageType;
            var retryBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(typedMessage));
            var memoryBody = new ReadOnlyMemory<byte>(retryBody);

            _channel.BasicPublish(
                exchange: "",
                routingKey: _settings.QueueName,
                mandatory: false,
                basicProperties: properties,
                body: memoryBody
            );
            _logger.LogWarning(ex, $"An error occurred. Retrying message. Retry count: {typedMessage.RetryCount}");
        }
    }
}

