namespace Poliedro.Eds.Application.Shopping.Dtos;

public record ShoppingInventoryDto
(
    int IdInventory,
    DateOnly Date,
    string ReferenceType,
    int ReferenceId
);