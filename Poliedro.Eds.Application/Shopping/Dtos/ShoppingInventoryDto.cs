using Poliedro.Eds.Domain.Common.Enums;

namespace Poliedro.Eds.Application.Shopping.Dtos;

public record ShoppingInventoryDto
(
    int IdInventory,
    DateOnly Date,
    ReferenceType ReferenceType,
    int ReferenceId
);
