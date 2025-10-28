using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Wizard.Commands.CreateSetup;

public record CreateSetupCommand(CreateSetupRequestDto Request) : IRequest<Result<VoidResult, Error>>;
