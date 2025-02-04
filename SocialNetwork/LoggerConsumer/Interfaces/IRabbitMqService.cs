﻿using RabbitMQ.Client;
using SocialNetwork.Infrastructure.Services.MessageQueue;

namespace LoggerService.Interfaces
{
    public interface IRabbitMqService
    {
        IConnectionFactory GetRabbirMqConnectionFactory();
        void Consume();
    }
}
