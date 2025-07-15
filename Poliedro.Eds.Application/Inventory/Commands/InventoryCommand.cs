namespace Poliedro.Eds.Application.Inventory.Commands;

public record InventoryCommand
(
    DateOnly Date,
    string ReferenceType,
    int ReferenceId
);