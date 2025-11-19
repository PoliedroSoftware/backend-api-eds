using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos.CompartimentDashboardView;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllCompartimentDashboardView;

public record GetAllCompartimentDashboardViewQuery(PaginationParams PaginationParams) 
    : IRequest<IEnumerable<CompartimentDashboardViewDto>>;
