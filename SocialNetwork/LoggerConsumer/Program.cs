// See https://aka.ms/new-console-template for more information

using LoggerConsumer.Concrete;
using LoggerConsumer.Interfaces;

IRabbitMqService consumer = new RabbitMqService();

while (1 == 1)
{
    consumer.Consume();
}