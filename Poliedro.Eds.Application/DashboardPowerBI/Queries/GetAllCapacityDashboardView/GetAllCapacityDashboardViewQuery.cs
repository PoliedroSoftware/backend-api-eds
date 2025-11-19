using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.CapacityDashboardView;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllCapacityDashboardView;

public record GetAllCapacityDashboardViewQuery(PaginationParams PaginationParams) 
    : IRequest<IEnumerable<CapacityDashboardViewDto>>;
