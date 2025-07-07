

namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;

public record ShoppingInventoryCommand
(
    DateOnly Date,
    string ReferenceType,
    int ReferenceId
);