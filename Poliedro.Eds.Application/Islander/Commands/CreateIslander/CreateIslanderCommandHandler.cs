using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Islander.Entities;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using System.Threading.Channels;
using Microsoft.Extensions.Configuration;

namespace Poliedro.Eds.Application.Islander.Commands.CreateIslander
{
    public class CreateIslanderCommandHandler(
        IIslanderCreateIslander islanderDomainIslander,
        IMapper mapper, IConfiguration config) : IRequestHandler<CreateIslanderCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateIslanderCommand request, CancellationToken cancellationToken)
        {
            var islanderEntity = mapper.Map<IslanderEntity>(request.Request);

            var originalPassword = islanderEntity.Password;

            islanderEntity.Password = BCrypt.Net.BCrypt.HashPassword(islanderEntity.Password);

            var dbResult = await islanderDomainIslander.CreateAsync(islanderEntity);
            if (!dbResult.IsSuccess)
                return dbResult.Error!;

            var factory = new ConnectionFactory() { HostName = config["RabbitMQ:HostName"], UserName = config["RabbitMQ:UserName"], Password = config["RabbitMQ:Password"] };
            using var rabbitConnection = factory.CreateConnection();
            using var channel = rabbitConnection.CreateModel();
  
            channel.ExchangeDeclare(exchange: "keycloak_exchange", type: ExchangeType.Direct);
            channel.QueueDeclare(queue: "keycloak", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "keycloak", exchange: "keycloak_exchange", routingKey: "keycloak");

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

            
            channel.BasicPublish(
                exchange: "keycloak_exchange",     
                routingKey: "keycloak",         
                basicProperties: properties,        
                body: body                         
            );

            Console.WriteLine("Mensaje enviado a la cola keycloak_user");
        

            return VoidResult.Instance;
        }
    }
}



