using RabbitMQ.Client;
using WorkerS3UploaderService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.Sources.Clear();

        var basePath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..", "Poliedro.Eds.Api");
        var apiSettingsPath = Path.Combine(basePath, "appsettings.json");

        if (!File.Exists(apiSettingsPath))
        {
            throw new FileNotFoundException($"No se encontró el appsettings.json del proyecto principal en: {apiSettingsPath}");
        }

        config
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddSingleton<IConnection>(sp =>
        {
            ;

            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };
            return factory.CreateConnection();
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
