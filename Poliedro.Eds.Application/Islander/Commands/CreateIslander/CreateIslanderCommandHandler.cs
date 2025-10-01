using System.Net;
using System.Text;
using System.Text.Json;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using RabbitMQ.Client;

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander
{
    public class CreateIslanderCommandHandler(
        IIslanderCreateIslander islanderDomainIslander,
        IMapper mapper,
        IValidator<CreateIslanderRequestDto> validator,
        IRedisService redisService,
        IConnection rabbitConnection) : IRequestHandler<CreateIslanderCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateIslanderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var islanderEntity = mapper.Map<IslanderEntity>(request.Request);

            var nameClaimToken = request.NameClaimToken;

            Console.WriteLine($"nombre del clain del token: {nameClaimToken}");

            var originalPassword = islanderEntity.Password;

            islanderEntity.Password = BCrypt.Net.BCrypt.HashPassword(islanderEntity.Password);

            var result = await islanderDomainIslander.CreateAsync(islanderEntity);
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.ISLANDER);

            if (!result.IsSuccess)
                return result.Error!;

            using var channel = rabbitConnection.CreateModel();
            channel.ExchangeDeclare("keycloak_exchange", ExchangeType.Direct);
            channel.QueueDeclare("keycloak", true, false, false, null);
            channel.QueueBind("keycloak", "keycloak_exchange", "keycloak");

            var message = new
            {
                islanderEntity.IdEds,
                islanderEntity.Name,
                islanderEntity.Email,
                islanderEntity.FirstName,
                islanderEntity.LastName,
                Password = originalPassword,
                
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish("keycloak_exchange", "keycloak", properties, body);
            Console.WriteLine("Mensaje enviado a la cola keycloak_user");

            return result.Value!;
        }
    }
}



