using RabbitMQ.Client;

namespace SocialNetwork.Infrastructure.Services.MessageQueue
{
    public interface IRabbitMqService
    {
        IConnectionFactory GetRabbirMqConnectionFactory();
        void Puslish(MessageQueueType message);
    }
}
