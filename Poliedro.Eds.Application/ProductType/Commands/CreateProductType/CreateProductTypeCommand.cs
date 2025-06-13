using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.ProductType.Commands.CreateProductType;

public record CreateProductTypeCommand (CreateProductTypeRequestDto Request) : IRequest<Result<VoidResult, Error>>;
