using MediatR;
using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.Inventory.Dto.View;

namespace Poliedro.Eds.Application.Inventory.Queries.GetInventoryList;

public record GetInventoryListQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<InventoryListResponseDto>>;
