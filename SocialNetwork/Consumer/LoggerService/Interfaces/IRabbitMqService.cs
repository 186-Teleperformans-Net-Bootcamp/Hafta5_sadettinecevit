using RabbitMQ.Client;

namespace LoggerService.Interfaces
{
    public interface IRabbitMqService
    {
        IConnectionFactory GetRabbirMqConnectionFactory();
        void Consume();
    }
}
