using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander
{
    public class CreateIslanderCommandHandler(
        IIslanderCreateIslander islanderDomainIslander,
        IMapper mapper,
        IConnection rabbitConnection) : IRequestHandler<CreateIslanderCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateIslanderCommand request, CancellationToken cancellationToken)
        {
            var islanderEntity = mapper.Map<IslanderEntity>(request.Request);

            var originalPassword = islanderEntity.Password;

            islanderEntity.Password = BCrypt.Net.BCrypt.HashPassword(islanderEntity.Password);

            var dbResult = await islanderDomainIslander.CreateAsync(islanderEntity);
            if (!dbResult.IsSuccess)
                return dbResult.Error!;

            using var channel = rabbitConnection.CreateModel();

            var message = new
            {
                islanderEntity.IdEds,
                islanderEntity.Name,
                islanderEntity.Email,
                islanderEntity.FirstName,
                islanderEntity.LastName,
                Password = originalPassword
            };

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "keycloak_exchange",
                                 routingKey: "keycloak_user",
                                 basicProperties: properties,
                                 body: body);

            return VoidResult.Instance;
        }
    }
}



