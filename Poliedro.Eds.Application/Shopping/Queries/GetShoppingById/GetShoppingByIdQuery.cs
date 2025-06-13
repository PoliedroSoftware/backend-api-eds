using MediatR;
using Poliedro.Eds.Application.Shopping.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Shopping.Queries.GetShoppingById;
public record GetShoppingByIdQuery(int Id) : IRequest<Result<ShoppingDto, Error>>;
