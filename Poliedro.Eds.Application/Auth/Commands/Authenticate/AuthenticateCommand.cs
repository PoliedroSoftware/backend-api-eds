using MediatR;
using Poliedro.Eds.Application.Auth.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Auth.Commands.Authenticate;

public record AuthenticateCommand(AuthenticateRequestDto Request) 
    : IRequest<Result<AuthenticateResponseDto, Error>>;
