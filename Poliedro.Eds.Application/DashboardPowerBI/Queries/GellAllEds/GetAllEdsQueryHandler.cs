using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.DashboardPowerBI.EdsView.DomainEds;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllEds;
public class GetAllEdsQueryHandler
(
    IEdsViewGetAllService EdsViewGetAllService,
    IMapper mapper)
    : IRequestHandler<GellAllEdsQuery, IEnumerable<EdsDto>>
{
    public async Task<IEnumerable<EdsDto>> Handle(GellAllEdsQuery request, CancellationToken cancellationToken)
    {
        var result = await EdsViewGetAllService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<EdsDto>>(result);
    }
}