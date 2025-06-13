using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;

namespace Poliedro.Eds.Application.Island.Commands.CreateIsland
{
    public class CreateIslandCommandHandler(
        IIslandCreateIsland islandDomainIsland,
        IMapper mapper) : IRequestHandler<CreateIslandCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateIslandCommand request, CancellationToken cancellationToken)
        {
            var islandEntity = mapper.Map<IslandEntity>(request.Request);
            var result = await islandDomainIsland.CreateAsync(islandEntity);
            if (!result.IsSuccess)
                return result.Error!;
            return result.Value!;
        }
    }
}
