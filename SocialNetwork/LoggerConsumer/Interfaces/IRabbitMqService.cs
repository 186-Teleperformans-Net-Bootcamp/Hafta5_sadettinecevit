using RabbitMQ.Client;
using SocialNetwork.Infrastructure.Services.MessageQueue;

namespace LoggerConsumer.Interfaces
{
    public interface IRabbitMqService
    {
        IConnectionFactory GetRabbirMqConnectionFactory();
        void Consume();
    }
}
