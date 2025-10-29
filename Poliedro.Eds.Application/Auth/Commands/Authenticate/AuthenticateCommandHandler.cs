using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Auth.Dtos;
using Poliedro.Eds.Domain.Auth.DomainAuth;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Auth.Commands.Authenticate;

public class AuthenticateCommandHandler(
    IKeycloakAuthService keycloakAuthService,
    IMapper mapper)
    : IRequestHandler<AuthenticateCommand, Result<AuthenticateResponseDto, Error>>
{
    public async Task<Result<AuthenticateResponseDto, Error>> Handle(
        AuthenticateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await keycloakAuthService.AuthenticateAsync(
                 request.Request.Username,
                 request.Request.Password);

        if (!result.IsSuccess)
        {
            return result.Error;
        }

        var response = mapper.Map<AuthenticateResponseDto>(result.Value);

        return response;
    }
}
