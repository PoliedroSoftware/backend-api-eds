using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.EdsTank.Commands.CreateEdsTank;

public record CreateEdsTankCommand (CreateEdsTankRequestDto Request) : IRequest<Result<VoidResult, Error>>;