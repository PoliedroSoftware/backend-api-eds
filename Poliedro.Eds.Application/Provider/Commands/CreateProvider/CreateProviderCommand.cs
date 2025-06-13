using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Provider.Commands.CreateProvider;

public record CreateProviderCommand(CreateProviderRequestDto Request) : IRequest<Result<VoidResult, Error>>;