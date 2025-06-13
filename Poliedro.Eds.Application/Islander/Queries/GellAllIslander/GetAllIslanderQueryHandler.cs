using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Islander.DomainIslander;

namespace Poliedro.Eds.Application.Islander.Queries.GellAllIslander;
public class GetAllIslanderQueryHandler
(
    IIslanderGetAllIslander islanderDomainIslander,
    IMapper mapper)
    : IRequestHandler<GellAllIslanderQuery, IEnumerable<IslanderDto>>
{
    public async Task<IEnumerable<IslanderDto>> Handle(GellAllIslanderQuery request, CancellationToken cancellationToken)
    {
        var result = await islanderDomainIslander.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<IslanderDto>>(result);
    }
}

