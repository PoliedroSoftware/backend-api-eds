namespace Poliedro.Eds.Application.Product.Dtos;

public record ProductDto(int IdProduct,
    string Name,
    int IdProductType,
    double? SellPrice,
    double? PurchasePrice,
    double? Stock,
    string? NameEDS);
