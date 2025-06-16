using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.DashboardPowerBI.DomainDashboardPowerBI;
namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllDashboardPowerBI;

public class GetAllDashboardPowerBIQueryHandler(
    IMasterGetAllService MasterGetAllService,
    IMapper mapper)
    : IRequestHandler<GetAllDashboardPowerBIQuery, MasterDto>
{
    public async Task<MasterDto> Handle(GetAllDashboardPowerBIQuery request, CancellationToken cancellationToken)
        => mapper.Map<MasterDto>(await MasterGetAllService.GetAllAsync(request.PaginationParams, cancellationToken));
}
