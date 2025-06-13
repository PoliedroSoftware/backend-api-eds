using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.HoseHistory.Dtos;
using Poliedro.Eds.Domain.HoseHistory.DomainHoseHistory;

namespace Poliedro.Eds.Application.HoseHistory.Queries.GellAllHoseHistory;
public class GetAllHoseHistoryQueryHandler
(
    IHoseHistoryGetAllHoseHistory hosehistoryDomainHoseHistory,
    IMapper mapper)
    : IRequestHandler<GellAllHoseHistoryQuery, IEnumerable<HoseHistoryDto>>
{
    public async Task<IEnumerable<HoseHistoryDto>> Handle(GellAllHoseHistoryQuery request, CancellationToken cancellationToken)
    {
        var result = await hosehistoryDomainHoseHistory.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<HoseHistoryDto>>(result);
    }
}

