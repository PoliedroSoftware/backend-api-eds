using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Compartiment.Commands.CreateCompartiment;

public record CreateCompartimentCommand(CreateCompartimentRequestDto Request) : IRequest<Result<VoidResult, Error>>;