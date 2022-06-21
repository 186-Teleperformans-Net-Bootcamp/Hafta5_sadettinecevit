using RabbitMQ.Client;

namespace SocialNetwork.Infrastructure.Services.RabbitMq
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        public IConnection GetRabbirMqConnection()
        {
            return new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            }.CreateConnection();
        }
    }
}
