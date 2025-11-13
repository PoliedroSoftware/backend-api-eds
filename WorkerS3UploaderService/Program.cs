using RabbitMQ.Client;
using WorkerS3UploaderService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.Sources.Clear();

        // Usar el appsettings.json local del WorkerService
        config
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettingsS3.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE") ?? configuration["RabbitMQ:HostName"],
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? configuration["RabbitMQ:UserName"],
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? configuration["RabbitMQ:Password"]
            };
            return factory.CreateConnection();
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
