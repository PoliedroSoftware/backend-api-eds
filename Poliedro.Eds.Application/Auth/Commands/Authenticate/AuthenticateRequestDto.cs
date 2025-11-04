namespace Poliedro.Eds.Application.Auth.Commands.Authenticate;

public record AuthenticateRequestDto(
    string Username,
    string Password);
