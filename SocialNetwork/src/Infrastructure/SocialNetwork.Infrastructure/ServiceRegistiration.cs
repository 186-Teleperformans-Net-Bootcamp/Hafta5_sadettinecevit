using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ;
using RabbitMQ.Client;
using SocialNetwork.Infrastructure.Services.RabbitMq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure
{
    public static class ServiceRegistiration
    {
        public static void AddInfrastructureService(this IServiceCollection serviceCollection, IConfiguration configuration = null)
        {
            serviceCollection.AddTransient<IRabbitMqConnection, RabbitMqConnection>();
        }
    }
}
