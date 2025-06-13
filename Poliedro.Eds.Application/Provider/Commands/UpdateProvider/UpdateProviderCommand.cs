using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Provider.Commands.UpdateProvider;

public record UpdateProviderCommand(int IdProvider, string Name) : IRequest<Result<VoidResult, Error>>;