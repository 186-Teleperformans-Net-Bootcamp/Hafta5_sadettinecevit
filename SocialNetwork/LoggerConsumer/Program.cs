// See https://aka.ms/new-console-template for more information

using LoggerService.Concrete;
using LoggerService.Interfaces;

IRabbitMqService consumer = new RabbitMqService();

while (1 == 1)
{
    consumer.Consume();
}