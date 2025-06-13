using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Commands.UpdateBusiness;

public record UpdateBusinessCommand(int IdBusiness, string Name) : IRequest<Result<VoidResult, Error>>;