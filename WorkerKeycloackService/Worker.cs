using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
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

                    var islanderDto = JsonSerializer.Deserialize<IslanderDto>(message);
                    var islanderEntity = new IslanderEntity
                    {
                        Name = islanderDto.Name,
                        Email = islanderDto.Email,
                        FirstName = islanderDto.FirstName,
                        LastName = islanderDto.LastName,
                        IdEds = islanderDto.IdEds,
                        Password = islanderDto.Password,
                    };

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var keycloakUserService = scope.ServiceProvider.GetRequiredService<IKeycloakUserService>();
                        var resultService = await keycloakUserService.CreateUserAsync(islanderEntity, islanderDto.Password);

                        if (resultService.IsSuccess)
                        {
                            _logger.LogInformation($"Usuario creado en Keycloak: {islanderDto.Email}");
                            channel.BasicAck(result.DeliveryTag, false);
                        }
                        else
                        {
                            _logger.LogError($"Error creando usuario: {resultService.Error}");
                            channel.BasicNack(result.DeliveryTag, false, true);
                        }
                    }
                }
                else
                {
                    _logger.LogInformation("No hay mensajes en la cola.");
                }

                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogInformation("Worker detenido.");
        }

    }
}
