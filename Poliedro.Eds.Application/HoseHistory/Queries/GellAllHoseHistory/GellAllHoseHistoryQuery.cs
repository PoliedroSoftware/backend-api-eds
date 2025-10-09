using MediatR;
using Poliedro.Eds.Application.HoseHistory.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.HoseHistory.Queries.GellAllHoseHistory;

public record GellAllHoseHistoryQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<HoseHistoryDto>>;
