using RabbitMQ.Client;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using WorkerKeycloackService;
using Poliedro.Eds.Infraestructure.External.Keycloak.Services;
using System.Net.Http.Headers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettingsworker.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        services.AddSingleton<IConnection>(sp =>
        {;

            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };
            return factory.CreateConnection();
        });

        services.AddHostedService<Worker>();

        services.AddHttpClient<IKeycloakUserService, KeycloakService>(client =>
        {

            client.BaseAddress = new Uri(configuration["Keycloak:KeycloakUri"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

    })
    .Build();

await host.RunAsync();
