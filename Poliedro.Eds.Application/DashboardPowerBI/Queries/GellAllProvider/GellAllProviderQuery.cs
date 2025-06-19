using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllProvider;

public record GellAllProviderQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<ProviderDto>>;