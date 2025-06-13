namespace Poliedro.Eds.Application.Product.Commands.CreateProduct;

public record CreateProductRequestDto(string Name, int IdProductType, Double Price);