using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;
using Poliedro.Eds.Domain.Island.Entities;
namespace Poliedro.Eds.Application.Island.Commands.UpdateIsland
{
    public class UpdateIslandCommandHandler(
        IIslandUpdateIsland islandDomainIsland,
        IMapper mapper
    ) : IRequestHandler<UpdateIslandCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateIslandCommand request, CancellationToken cancellationToken)
        {
            var islandEntity = mapper.Map<IslandEntity>(request);
            var result = await islandDomainIsland.UpdateAsync(islandEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }
}