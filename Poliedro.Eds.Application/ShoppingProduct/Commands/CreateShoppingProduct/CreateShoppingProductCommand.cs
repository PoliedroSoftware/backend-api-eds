using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
public record CreateShoppingProductCommand(CreateShoppingProductRequestDto Request) : IRequest<Result<VoidResult, Error>>;