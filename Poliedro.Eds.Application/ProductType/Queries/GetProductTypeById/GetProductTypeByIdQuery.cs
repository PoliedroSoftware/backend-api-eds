using MediatR;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.ProductType.Queries.GetProductTypeById;
    public record GetProductTypeByIdQuery (int Id) : IRequest<Result<ProductTypeDto, Error>>;
