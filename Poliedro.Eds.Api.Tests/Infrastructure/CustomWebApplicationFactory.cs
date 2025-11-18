using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Poliedro.Eds.Application.Ports.Translations;

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
    { "Keycloak:Auth", "http://localhost:8080/realms/TestRealm" },
         { "Keycloak:ClientId", "test-client" },
   { "Keycloak:DefaultGroupId", "test-group-id" },
      { "MYSQL_CONNECTION", "Server=localhost;Database=test;User=test;Password=test;" },
   { "ConnectionStrings:MysqlConnection", "Server=localhost;Database=test;User=test;Password=test;" },
  { "Redis:ConnectionString", "localhost:6379,abortConnect=false,connectTimeout=5000,syncTimeout=5000" },
  { "RabbitMQ:Enabled", "false" },
{ "RabbitMQ:HostName", "localhost" },
         { "RabbitMQ:Port", "5672" },
   { "RabbitMQ:UserName", "guest" },
     { "RabbitMQ:Password", "guest" },
     { "RabbitMQ:Queue", "test-queue" },
  { "OpenAI:ApiKey", "test-api-key" },
{ "Tolgee:BaseUrl", "http://localhost:8080" },
   { "Tolgee:ApiKey", "test-api-key" }
      };

   config.AddInMemoryCollection(configValues!);
      });

        builder.ConfigureServices(services =>
    {
   // Remove JWT Bearer authentication completely for tests
   var jwtScheme = services.FirstOrDefault(d => 
          d.ServiceType == typeof(IAuthenticationService) ||
        d.ServiceType == typeof(IAuthenticationSchemeProvider) ||
  d.ImplementationType?.FullName?.Contains("JwtBearer") == true);
   
   // Remove IConnection (RabbitMQ) registration and replace with mock
  var rabbitConnection = services.FirstOrDefault(d => 
          d.ServiceType == typeof(RabbitMQ.Client.IConnection));
   if (rabbitConnection != null)
            {
services.Remove(rabbitConnection);
    }

 // Add mock IConnection that doesn't throw NullReferenceException
  var mockConnection = new Mock<RabbitMQ.Client.IConnection>();
  mockConnection.Setup(x => x.IsOpen).Returns(false);
 mockConnection.Setup(x => x.Dispose()).Verifiable();
    services.AddSingleton<RabbitMQ.Client.IConnection>(mockConnection.Object);

   // Remove and mock ITolgeeService to prevent external HTTP calls
   var tolgeeService = services.FirstOrDefault(d => d.ServiceType == typeof(ITolgeeService));
   if (tolgeeService != null)
   {
       services.Remove(tolgeeService);
   }

   // Add mock ITolgeeService with test data
   var mockTolgeeService = new Mock<ITolgeeService>();
   mockTolgeeService.Setup(x => x.GetAllTranslationsFromTolgee())
       .ReturnsAsync(new Dictionary<string, Dictionary<string, string>>
       {
           ["en"] = new Dictionary<string, string>
           {
               ["test.key"] = "Test Value",
               ["app.title"] = "Test Application"
           },
           ["es-CO"] = new Dictionary<string, string>
           {
               ["test.key"] = "Valor de Prueba",
               ["app.title"] = "Aplicación de Prueba"
           }
       });
   services.AddSingleton<ITolgeeService>(mockTolgeeService.Object);

   // Remove all hosted services (Workers) that depend on RabbitMQ or external services
      var hostedServices = services.Where(d =>
   d.ServiceType == typeof(IHostedService) &&
     (d.ImplementationType?.Name.Contains("Worker") == true ||
 d.ImplementationType?.Name.Contains("CacheInvalidation") == true ||
  d.ImplementationType?.Name.Contains("Translation") == true ||
 d.ImplementationType?.Name.Contains("CachingService") == true))
      .ToList();

  foreach (var service in hostedServices)
     {
 services.Remove(service);
 }

     // Remove HealthCheck services that require external dependencies
 var healthCheckServices = services.Where(d =>
  d.ServiceType.Name.Contains("HealthCheck") &&
  (d.ImplementationType?.Name.Contains("Tolgee") == true ||
         d.ImplementationType?.Name.Contains("WhatsApp") == true ||
  d.ImplementationType?.Name.Contains("MySql") == true ||
         d.ImplementationType?.Name.Contains("Redis") == true))
         .ToList();

foreach (var service in healthCheckServices)
   {
    services.Remove(service);
     }

        // Remove AWS Logger that might fail in CI
    var awsLoggers = services.Where(d => 
      d.ServiceType.Name.Contains("AWS") ||
   d.ServiceType.Name.Contains("Logger"))
  .ToList();

     foreach (var service in awsLoggers)
   {
 services.Remove(service);
  }

 // Ensure HttpContextAccessor is available for tests
 if (!services.Any(d => d.ServiceType == typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor)))
    {
   services.AddHttpContextAccessor();
    }

   // Override JWT Bearer configuration to disable HTTPS requirement
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
  {
      options.RequireHttpsMetadata = false;
      options.Authority = null;
       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
      {
    ValidateIssuer = false,
    ValidateAudience = false,
     ValidateLifetime = false,
           ValidateIssuerSigningKey = false
     };
       });
        });

     builder.UseEnvironment("Test");
    
  // Suppress startup errors that might occur in test environment
    builder.ConfigureLogging(logging =>
    {
  logging.ClearProviders();
          logging.AddConsole();
  logging.AddFilter("Microsoft", LogLevel.Warning);
      logging.AddFilter("System", LogLevel.Warning);
  });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
  // Ensure proper cleanup of resources
     try
    {
   base.Dispose(disposing);
   }
      catch
    {
       // Suppress disposal errors in tests
    }
 }
 }
}
