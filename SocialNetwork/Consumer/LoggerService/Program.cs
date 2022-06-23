using LoggerService;
using LoggerService.Concrete;
using LoggerService.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(service => service.ServiceName = "SocialNetwork Logger Service")
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
    })
    .Build();

await host.RunAsync();
