using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.RegisterShift.Commands.CreateRegisterShift;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class RegisterShiftEndpoints
{
    public static IEndpointRouteBuilder MapRegisterShiftEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/register-shift")
            .WithTags("RegisterShift")
            .RequireAuthorization("AdminOrIslander");

        group.MapPost("", Create).WithName("CreateRegisterShift").WithSummary("Create new register shift");

        return app;
    }

    private static async Task<IResult> Create([FromBody] CreateRegisterShiftCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created($"/api/v1/register-shift", result.Value),
            onFailure => TypedResults.BadRequest(onFailure));
    }
}
