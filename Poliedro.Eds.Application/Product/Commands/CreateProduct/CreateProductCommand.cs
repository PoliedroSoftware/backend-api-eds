using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Product.Commands.CreateProduct;

public record CreateProductCommand (CreateProductRequestDto Request) : IRequest<Result<VoidResult, Error>>;
