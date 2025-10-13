using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Domain.Islander.Events;
using RabbitMQ.Client;
namespace WorkerKeycloackService
{
    public class Worker(
        IConnection _rabbitConnection,
        ILogger<Worker> _logger,
        IServiceProvider _serviceProvider,
        IConfiguration _configuration) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = _rabbitConnection.CreateModel();

            var queueName = _configuration["RabbitMQ:Queue"];

            channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _logger.LogInformation($"Escuchando la cola '{queueName}' cada 5 segundos...");

            while (!stoppingToken.IsCancellationRequested)
            {
                
                var result = channel.BasicGet(queue: queueName, autoAck: false);

                if (result != null)
                {
                    var message = Encoding.UTF8.GetString(result.Body.ToArray());
                    _logger.LogInformation($"Mensaje recibido: {message}");

                    var islanderDto = JsonSerializer.Deserialize<IslanderMessageDto>(message);
                    var islanderEntity = new IslanderEntity
                    {
                        Name = islanderDto.User,
                        Email = islanderDto.Email,
                        FirstName = islanderDto.FirstName,
                        LastName = islanderDto.LastName,
                        IdEds = islanderDto.IdEds,
                        Password = islanderDto.Password,
                    };

                    using var scope = _serviceProvider.CreateScope();
                    var keycloakUserService = scope.ServiceProvider.GetRequiredService<IKeycloakUserService>();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    
                    var resultService = await keycloakUserService.CreateUserAsync(islanderEntity, islanderDto.Password, islanderDto.NameClaimToken);

                    if (resultService.IsSuccess)
                    {
                        _logger.LogInformation($"Usuario creado en Keycloak: {islanderDto.Email}");
                        var tenant = islanderDto.Tenant ?? string.Empty;
                        var keycloakCreatedEvent = new IslanderKeycloakCreatedEvent(islanderEntity, tenant);
                        
                        await mediator.Publish(keycloakCreatedEvent, stoppingToken);
                        _logger.LogInformation($"Evento IslanderKeycloakCreatedEvent publicado para: {islanderDto.Email} con tenant: {tenant}");
                        
                        channel.BasicAck(result.DeliveryTag, false);
                    }
                    else
                    {
                        _logger.LogError($"Error creando usuario: {resultService.Error}");
                        channel.BasicNack(result.DeliveryTag, false, requeue: true);
                    }
                }
                else
                {
                    _logger.LogInformation("No hay mensajes en la cola.");
                }
                var delay = _configuration.GetValue<int>("worker:PollingInterval", 30000);
                await Task.Delay(30000, stoppingToken);
            }

            _logger.LogInformation("Worker detenido.");
        }

    }
}
