using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;

namespace Poliedro.Eds.Application.Island.Commands.CreateIsland
{
    public class CreateIslandCommandHandler(
        IIslandCreateIsland islandDomainIsland,
        IMapper mapper,
        IValidator<CreateIslandRequestDto> validator
        ) : IRequestHandler<CreateIslandCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateIslandCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.Request);
            if (!validationResult.IsValid)
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));

            var islandEntity = mapper.Map<IslandEntity>(request.Request);
            var result = await islandDomainIsland.CreateAsync(islandEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
