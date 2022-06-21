using RabbitMQ.Client;

namespace SocialNetwork.Infrastructure.Services.RabbitMq
{
    public interface IRabbitMqConnection
    {
        IConnection GetRabbirMqConnection();
    }
}
