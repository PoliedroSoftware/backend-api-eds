using MediatR;
using Poliedro.Eds.Application.ProductCompartiment.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.ProductCompartiment.Queries.GetProductCompartimentById;
    public record GetProductCompartimentByIdQuery (int Id) : IRequest<Result<ProductCompartimentDto, Error>>;
