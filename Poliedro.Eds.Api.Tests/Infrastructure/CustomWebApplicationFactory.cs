using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Poliedro.Eds.Api.Tests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add in-memory configuration to override settings for testing
            var configValues = new Dictionary<string, string>
            {
                { "Keycloak:KeycloakUri", "http://localhost:8080" },
                { "Keycloak:Realm", "TestRealm" },
                { "Keycloak:DefaultGroupId", "test-group-id" },
                { "MYSQL_CONNECTION", "Server=localhost;Database=test;User=test;Password=test;" },
                { "RabbitMQ:Enabled", "false" },
                { "RabbitMQ:HostName", "localhost" },
                { "RabbitMQ:Port", "5672" },
                { "RabbitMQ:UserName", "guest" },
                { "RabbitMQ:Password", "guest" }
            };

            config.AddInMemoryCollection(configValues!);
        });

        builder.ConfigureServices(services =>
        {
            // Remove IConnection (RabbitMQ) registration
            var rabbitConnection = services.FirstOrDefault(d => d.ServiceType.Name.Contains("IConnection"));
            if (rabbitConnection != null)
            {
                services.Remove(rabbitConnection);
            }

            // Remove all hosted services (Workers) that depend on RabbitMQ
            var hostedServices = services.Where(d =>
                    d.ServiceType == typeof(IHostedService) &&
                    (d.ImplementationType?.Name.Contains("Worker") == true ||
                     d.ImplementationType?.Name.Contains("CacheInvalidation") == true ||
                     d.ImplementationType?.Name.Contains("Translation") == true))
                .ToList();

            foreach (var service in hostedServices)
            {
                services.Remove(service);
            }

            // Add mock IConnection for services that still might need it
            services.AddSingleton<RabbitMQ.Client.IConnection>(sp =>
            {
                // Return null to prevent actual RabbitMQ connections
                return null!;
            });
        });

        builder.UseEnvironment("Test");
    }
}
