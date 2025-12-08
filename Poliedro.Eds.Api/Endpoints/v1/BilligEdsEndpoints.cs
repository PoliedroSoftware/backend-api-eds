using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.BilligEds.Commands.CreateBilligEds;
using Poliedro.Eds.Domain.BilligEds.Entities;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class BilligEdsEndpoints
{
    public static IEndpointRouteBuilder MapBilligEdsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/billigeds").WithTags("BilligEds");

        group.MapPost("", Create)
            .WithName("CreateBilligEds")
            .WithSummary("Create and send billing electronic document")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> Create(
        [FromBody] BillidEdsRequestEntity request,
        IMediator mediator)
    {
        var command = new CreateBilligEdsCommand(request);
        var result = await mediator.Send(command);

        if (result.IsSuccess)
            return TypedResults.Created();

        return TypedResults.Json(new { status = (int)result.Error!.HttpStatusCode, type = result.Error.Code, message = result.Error.Description }, statusCode: (int)result.Error.HttpStatusCode);
    }
}
