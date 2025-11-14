using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander
{
    public class CreateIslanderCommandHandler(
        IMapper mapper,
        IValidator<CreateIslanderRequestDto> validator,
        IConnection rabbitConnection,
        IHttpContextAccessor httpContextAccessor,
        IIslanderGetByUserIslander islanderGetByUser,
        ILogger<CreateIslanderCommandHandler> logger
        ) : IRequestHandler<CreateIslanderCommand, bool>
    {
        public async Task<bool> Handle(CreateIslanderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
            {
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest);
                return false;
            }

            IslanderEntity islanderEntity = mapper.Map<IslanderEntity>(request.Request);

            Console.WriteLine($"nombre del clain del token: {request.NameClaimToken}");

            var exists = await islanderGetByUser.ExistsAsync(islanderEntity.Name);
            if (exists)
            {
                Console.WriteLine($"Usuario con nombre {islanderEntity.Name} ya existe. No se publicará en RabbitMQ.");
                return false;
            }

            // Validar conexión de RabbitMQ
            if (rabbitConnection == null || !rabbitConnection.IsOpen)
            {
                logger.LogWarning("RabbitMQ connection is not available. Islander creation will not be sent to queue.");
                return false;
            }

            try
            {
                var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
                
                using var channel = rabbitConnection.CreateModel();
                channel.ExchangeDeclare("keycloak_exchange", ExchangeType.Direct);
                channel.QueueDeclare("keycloak", true, false, false, null);
                channel.QueueBind("keycloak", "keycloak_exchange", "keycloak");

                var message = new IslanderMessageDto
                {
                    IdEds = islanderEntity.IdEds,
                    User = islanderEntity.Name,
                    Email = islanderEntity.Email,
                    FirstName = islanderEntity.FirstName,
                    LastName = islanderEntity.LastName,
                    Password = islanderEntity.Password,
                    NameClaimToken = request.NameClaimToken,
                    Tenant = tenant 
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish("keycloak_exchange", "keycloak", properties, body);
                Console.WriteLine("Mensaje enviado a la cola keycloak_user");

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing message to RabbitMQ for Islander creation");
                return false;
            }
        }
    }
}



