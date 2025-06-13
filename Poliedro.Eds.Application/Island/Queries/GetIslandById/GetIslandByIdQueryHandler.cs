using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Island.DomainIsland;

namespace Poliedro.Eds.Application.Island.Queries.GetIslandById
{
    public class GetIslandByIdQueryHandler(
        IIslandGetByIdIsland islandDomainIsland,
        IMapper mapper)
        : IRequestHandler<GetIslandByIdQuery, Result<IslandDto, Error>>
    {
        public async Task<Result<IslandDto, Error>> Handle(GetIslandByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await islandDomainIsland.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<IslandDto>(result.Value);
        }
    }
}
