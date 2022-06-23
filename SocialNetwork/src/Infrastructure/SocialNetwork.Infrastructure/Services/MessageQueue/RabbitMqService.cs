using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Infrastructure.Services.MessageQueue
{

    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnectionFactory _connection;
        public RabbitMqService()
        {
            _connection = GetRabbirMqConnectionFactory();
        }

        public IConnectionFactory GetRabbirMqConnectionFactory()
        {
            return new ConnectionFactory()
            {
                HostName = "localhost",
                VirtualHost = "/",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
        }
        //Loglama için kullanılacak.
        public void Puslish(MessageQueueType message)
        {
            using IConnection connection = _connection.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("fanout.logger", ExchangeType.Fanout, false, false);

            channel.QueueDeclare(
                queue: "fanout.loggerWorker", 
                durable: false,
                exclusive: false,
                autoDelete: false);
            channel.QueueBind("fanout.loggerWorker", "fanout.logger", string.Empty);

            var jsonString = JsonSerializer.Serialize(message);
            channel.BasicPublish("fanout.logger", string.Empty, null, Encoding.UTF8.GetBytes(jsonString));

            channel.Close();
            connection.Close();
        }
    }
}
