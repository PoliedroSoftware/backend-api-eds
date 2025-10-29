using AutoMapper;
using Poliedro.Eds.Application.Auth.Dtos;
using Poliedro.Eds.Domain.Auth.DomainAuth;

namespace Poliedro.Eds.Application.Auth.AutoMappers;

public class AuthMapper : Profile
{
    public AuthMapper()
    {
        CreateMap<KeycloakTokenResult, AuthenticateResponseDto>();
    }
}
