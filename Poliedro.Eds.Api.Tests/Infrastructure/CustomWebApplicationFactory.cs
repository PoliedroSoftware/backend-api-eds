using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                { "MYSQL_CONNECTION", "Server=localhost;Database=test;User=test;Password=test;" }
            };

            config.AddInMemoryCollection(configValues!);
        });

        builder.ConfigureServices(services =>
        {
            // Additional service configurations can be added here
        });

        builder.UseEnvironment("Test");
    }
}
