using MediatR;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Product.Queries.GetProductById;
    public record GetProductByIdQuery (int Id) : IRequest<Result<ProductDto, Error>>;
