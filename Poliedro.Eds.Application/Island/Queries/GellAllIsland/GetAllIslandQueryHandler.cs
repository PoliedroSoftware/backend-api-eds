using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Domain.Island.DomainIsland;

namespace Poliedro.Eds.Application.Island.Queries.GellAllIsland;
public class GetAllIslandQueryHandler
(
    IIslandGetAllIsland islandDomainIsland,
    IMapper mapper)
    : IRequestHandler<GellAllIslandQuery, IEnumerable<IslandDto>>
{
    public async Task<IEnumerable<IslandDto>> Handle(GellAllIslandQuery request, CancellationToken cancellationToken)
    {
        var result = await islandDomainIsland.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<IslandDto>>(result);
    }
}

