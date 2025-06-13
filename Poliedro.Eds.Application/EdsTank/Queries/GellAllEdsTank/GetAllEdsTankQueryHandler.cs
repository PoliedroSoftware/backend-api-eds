using System.Diagnostics.Metrics;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.EdsTank.Dtos;
using Poliedro.Eds.Domain.EdsTank.DomainEdsTank;

namespace Poliedro.Eds.Application.EdsTank.Queries.GellAllEdsTank;
public class GetAllEdsTankQueryHandler
(
    IEdsTankGetAllEdsTank EdsTankDomainEdsTank,
    IMapper mapper)
    : IRequestHandler<GellAllEdsTankQuery, IEnumerable<EdsTankDto>>
{
    public async Task<IEnumerable<EdsTankDto>> Handle(GellAllEdsTankQuery request, CancellationToken cancellationToken)
    {
        var result = await EdsTankDomainEdsTank.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<EdsTankDto>>(result);
    }
}


