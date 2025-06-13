using MediatR;
using Poliedro.Eds.Application.CourtDispensersInventory.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.CourtDispensersInventory.Queries.GellAllCourtDispensersInventory;

public record GellAllCourtDispensersInventoryQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<CourtDispensersInventoryDto>>;
