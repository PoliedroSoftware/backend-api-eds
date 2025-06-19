using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Provider.DomainProvider;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllProvider;
public class GetAllProviderQueryHandler
(
    IProviderGetAllService ProviderGetAllService,
    IMapper mapper)
    : IRequestHandler<GellAllProviderQuery, IEnumerable<ProviderDto>>
{
    public async Task<IEnumerable<ProviderDto>> Handle(GellAllProviderQuery request, CancellationToken cancellationToken)
    {
        var result = await ProviderGetAllService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<ProviderDto>>(result);
    }
}