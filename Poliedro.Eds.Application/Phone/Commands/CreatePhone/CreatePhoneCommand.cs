using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Phone.Commands.CreatePhone;

public record CreatePhoneCommand(CreatePhoneRequestDto Request) : IRequest<Result<VoidResult, Error>>;
