using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.DomainProviderView;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllProvider;

public class GetAllProviderQueryHandler
(
    IProviderViewGetAllService ProviderViewGetAllService,
    IMapper mapper)
    : IRequestHandler<GellAllProviderQuery, IEnumerable<ProviderDto>>
{
    public async Task<IEnumerable<ProviderDto>> Handle(GellAllProviderQuery request, CancellationToken cancellationToken)
    {
        var result = await ProviderViewGetAllService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ProviderDto>>(result);
    }
}
