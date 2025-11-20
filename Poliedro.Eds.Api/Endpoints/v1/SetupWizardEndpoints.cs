using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Wizard.Commands.CreateSetup;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class SetupWizardEndpoints
{
    public static IEndpointRouteBuilder MapSetupWizardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/bootstrap/setup")
            .WithTags("SetupWizard")
            .RequireAuthorization("AdminOnly");

        group.MapPost("", Create).WithName("CreateSetup").WithSummary("Create new setup configuration");

        return app;
    }

    private static async Task<IResult> Create([FromBody] CreateSetupCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created($"/api/v1/bootstrap/setup", result.Value),
            onFailure => TypedResults.BadRequest(onFailure));
    }
}
