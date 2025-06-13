using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.CreateProductCompartiment;

public record CreateProductCompartimentCommand (CreateProductCompartimentRequestDto Request) : IRequest<Result<VoidResult, Error>>;