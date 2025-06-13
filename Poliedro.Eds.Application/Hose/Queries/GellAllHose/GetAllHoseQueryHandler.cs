using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Application.Hose.Queries.GellAllHose;
public class GetAllHoseQueryHandler
(
    IHoseGetAllHose hoseDomainHose,
    IMapper mapper)
    : IRequestHandler<GellAllHoseQuery, IEnumerable<HoseDto>>
{
    public async Task<IEnumerable<HoseDto>> Handle(GellAllHoseQuery request, CancellationToken cancellationToken)
    {
        var result = await hoseDomainHose.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<HoseDto>>(result);
    }
}

