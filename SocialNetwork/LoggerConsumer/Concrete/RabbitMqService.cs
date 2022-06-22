using LoggerConsumer.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SocialNetwork.Infrastructure.Services.MessageQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoggerConsumer.Concrete
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

        public void Consume()
        {
            using IConnection connection = _connection.CreateConnection();
            using IModel channel = connection.CreateModel();

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, args) =>
            {
                string messageString = Encoding.UTF8.GetString(args.Body.ToArray());
                MessageQueueType message = JsonSerializer.Deserialize<MessageQueueType>(messageString);
                string path = @"..\..\..\Log\Logs.txt"; //Directory.GetCurrentDirectory() +
                File.AppendAllLines(path, new string[] { message.EventTime.ToString() + " : ", message.EventDescription });
            };

            channel.BasicConsume("fanout.loggerWorker", true, consumer);
        }
    }
}
