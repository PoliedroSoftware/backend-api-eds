using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.ShoppingProduct.Queries.GetShoppingProductById;
public record GetShoppingProductByIdQuery(int Id) : IRequest<Result<ShoppingProductDto, Error>>;
