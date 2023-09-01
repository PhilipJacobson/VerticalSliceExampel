using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace VerticalSliceExample.CommonModule.Messaging
{
    public class MessagePublisher
    {
        public async Task PublishAsync<T>(T message, string queueName, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: durable,
                                     exclusive: exclusive,
                                     autoDelete: autoDelete,
                                     arguments: arguments);

                string messageJson = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageJson);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
