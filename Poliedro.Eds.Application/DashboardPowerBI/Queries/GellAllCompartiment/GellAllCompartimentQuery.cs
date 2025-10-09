using MediatR;
using Poliedro.Eds.Application.DashboardPowerBI.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.DashboardPowerBI.Queries.GellAllCompartiment;

public record GellAllCompartimentQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<CompartimentDto>>;
