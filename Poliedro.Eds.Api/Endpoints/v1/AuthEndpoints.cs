using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Auth.Commands.Authenticate;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/auth")
            .WithTags("Auth");

        group.MapPost("login", Login)
            .WithName("Login")
            .WithSummary("Authenticate user with Keycloak")
            .WithDescription("Authenticates a user with Keycloak and returns access tokens")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
            .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
            .AllowAnonymous();

        return app;
    }

    private static async Task<IResult> Login(
        [FromBody] AuthenticateRequestDto request,
        IMediator mediator)
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
