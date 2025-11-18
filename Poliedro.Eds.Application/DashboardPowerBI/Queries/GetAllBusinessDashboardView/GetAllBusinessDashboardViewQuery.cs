using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.BusinessDashboardView;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllBusinessDashboardView;

public record GetAllBusinessDashboardViewQuery(PaginationParams PaginationParams) 
    : IRequest<IEnumerable<BusinessDashboardViewDto>>;
