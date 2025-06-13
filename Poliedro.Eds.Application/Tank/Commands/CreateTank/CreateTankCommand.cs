using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Tank.Commands.CreateTank;

public record CreateTankCommand(CreateTankRequestDto Request) : IRequest<Result<VoidResult, Error>>;
