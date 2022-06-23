using LoggerService.Interfaces;
using LoggerService.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace LoggerService.Concrete
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
            using (IConnection connection = _connection.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    //channel.QueueDeclare("fanout.loggerWorker", false, false, false);
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (sender, args) =>
                    {
                        try
                        {
                            string messageString = Encoding.UTF8.GetString(args.Body.ToArray());
                            MessageQueueType message = JsonSerializer.Deserialize<MessageQueueType>(messageString);
                            string path = @"..\..\Consumer\LoggerService\Log\Logs.txt"; //Directory.GetCurrentDirectory() +
                            File.AppendAllLines(path, new string[] { message.EventTime.ToString() + " : ", message.EventDescription });
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.StackTrace.ToString());
                        }
                    };
                    channel.BasicConsume("fanout.loggerWorker", true, consumer);
                }
            }
        }
    }
}
