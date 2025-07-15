using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.DomainCapacityView;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllCapacity;
public class GetAllCapacityQueryHandler
(
    ICapacityViewGetAllService CapacityViewGetAllService,
    IMapper mapper)
    : IRequestHandler<GellAllCapacityQuery, IEnumerable<CapacityDto>>
{
    public async Task<IEnumerable<CapacityDto>> Handle(GellAllCapacityQuery request, CancellationToken cancellationToken)
    {
        var result = await CapacityViewGetAllService.GetAllAsync(request.PaginationParams);
        return mapper.Map<List<CapacityDto>>(result);
    }
}