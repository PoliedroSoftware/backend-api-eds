using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;

namespace Poliedro.Eds.Application.Island.Commands.CreateIsland
{
    public class CreateIslandCommandHandler(
        IIslandCreateIsland islandDomainIsland,
        IMapper mapper,
        IValidator<CreateIslandRequestDto> validator,
        IRedisService redisService
        ) : IRequestHandler<CreateIslandCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateIslandCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var result = await islandDomainIsland.CreateAsync(mapper.Map<IslandEntity>(request.Request));
            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.ISLAND);
            return result.IsSuccess ? result.Value! : result.Error!;
        }
    }
}
