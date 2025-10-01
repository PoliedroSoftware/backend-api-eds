using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Phone.Commands.UpdatePhone;

public record UpdatePhoneCommand(int IdPhone, string Number, string Name) : IRequest<Result<VoidResult, Error>>;
