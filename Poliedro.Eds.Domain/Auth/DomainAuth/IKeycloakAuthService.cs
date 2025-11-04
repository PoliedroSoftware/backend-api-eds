using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Domain.Auth.DomainAuth;

public interface IKeycloakAuthService
{
    Task<Result<KeycloakTokenResult, Error>> AuthenticateAsync(string username, string password);
}

public record KeycloakTokenResult(
    string AccessToken,
    int ExpiresIn,
    int RefreshExpiresIn,
    string RefreshToken,
    string TokenType,
    string Scope);
