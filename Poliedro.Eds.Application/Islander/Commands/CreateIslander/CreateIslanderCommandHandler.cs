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

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander
{
    public class CreateIslanderCommandHandler(
        IIslanderCreateIslander islanderDomainIslander,
        IMapper mapper,
        IValidator<CreateIslanderRequestDto> validator,
        IRedisService redisService,
        IDomainEventDispatcher domainEventDispatcher,
        IHttpContextAccessor httpContextAccessor,
        IConnection rabbitConnection) : IRequestHandler<CreateIslanderCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateIslanderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            IslanderEntity islanderEntity = mapper.Map<IslanderEntity>(request.Request);

            Console.WriteLine($"nombre del clain del token: {request.NameClaimToken}");

            string originalPassword = islanderEntity.Password;

            islanderEntity.Password = BCrypt.Net.BCrypt.HashPassword(islanderEntity.Password);

            var result = await islanderDomainIslander.CreateAsync(islanderEntity);
            
            await RedisHelper.InvalidateDistributedCacheAsync(
                result, 
                redisService, 
                domainEventDispatcher, 
                httpContextAccessor,
                "islander",
                "create",   
                islanderEntity.IdIslander);

            if (!result.IsSuccess)
                return result.Error!;

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
                NameClaimToken = request.NameClaimToken
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



