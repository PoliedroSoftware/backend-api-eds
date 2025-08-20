using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllCompartiment;

public record GellAllCompartimentQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<CompartimentDto>>;
