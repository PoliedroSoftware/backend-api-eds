using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.DomainBusinessView;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllBusiness;
public class GetAllBusinessQueryHandler
(
    IBusinessViewGetAllService BusinessViewGetAllService,
    IMapper mapper)
    : IRequestHandler<GellAllBusinessQuery, IEnumerable<Business2Dto>>
{
    public async Task<IEnumerable<Business2Dto>> Handle(GellAllBusinessQuery request, CancellationToken cancellationToken)
    {
        var result = await BusinessViewGetAllService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<Business2Dto>>(result);
    }
}