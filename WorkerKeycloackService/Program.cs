using System.Net.Http.Headers;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Infraestructure.External.Keycloak.Services;
using RabbitMQ.Client;
using WorkerKeycloackService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettingsworker.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTAME") ?? configuration["RabbitMQ:HostName"],
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? configuration["RabbitMQ:UserName"],
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? configuration["RabbitMQ:Password"]
            };
            return factory.CreateConnection();
        });

        services.AddHostedService<Worker>();

        services.AddHttpClient<IKeycloakUserService, KeycloakService>(client =>
        {

            client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("KEYCLOAK_URL") ?? configuration["Keycloak:KeycloakUri"]!);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

    })
    .Build();

await host.RunAsync();
