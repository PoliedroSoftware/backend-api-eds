using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Auth.Commands.Authenticate;
using Poliedro.Eds.Application.Common.Features;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Poliedro.Eds.Api.Controllers.v1.Auth;

[Route("api/v1/auth")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Authenticates a user with Keycloak and returns access tokens
    /// </summary>
    /// <param name="request">Authentication credentials</param>
    /// <returns>Access token, refresh token and authentication details</returns>
    /// <response code="200">Authentication successful, returns tokens</response>
    /// <response code="401">Authentication failed, invalid credentials</response>
    /// <response code="500">Internal server error</response>
    [SwaggerOperation(Summary = "Authenticate user with Keycloak")]
    [SwaggerResponse(StatusCodes.Status200OK, "Authentication successful", typeof(object))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IResult> Login([FromBody] AuthenticateRequestDto request)
    {
        var command = new AuthenticateCommand(request);
        var result = await mediator.Send(command);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => result.Error.HttpStatusCode switch
            {
                HttpStatusCode.Unauthorized => TypedResults.Unauthorized(),
                _ => TypedResults.BadRequest(onFailure)
            }
        );
    }
}
