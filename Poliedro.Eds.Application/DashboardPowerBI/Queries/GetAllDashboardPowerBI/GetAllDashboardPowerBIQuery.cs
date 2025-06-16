

using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GetAllDashboardPowerBI;

public record GetAllDashboardPowerBIQuery(PaginationParams PaginationParams) : IRequest<MasterDto>;

