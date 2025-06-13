using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.ShoppingProduct.Commands.UpdateShoppingProduct;

    public record UpdateShoppingProductCommand(
    int IdShoppingProduct,
    int IdShopping, 
    int IdProduct, 
    double Quantity,
    double Price,
    int IdCompartment) : IRequest<Result<VoidResult, Error>>;