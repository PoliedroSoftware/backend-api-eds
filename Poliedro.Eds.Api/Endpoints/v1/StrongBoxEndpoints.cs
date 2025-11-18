using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.StrongBox.Commands;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetById;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetByEds;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetList;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetTotalBalance;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class StrongBoxEndpoints
{
    public static IEndpointRouteBuilder MapStrongBoxEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/strong-box")
            .WithTags("StrongBox")
            .RequireAuthorization("AdminOrIslander");

        group.MapGet("", GetAll).WithName("GetAllStrongBoxes").WithSummary("Get all strong boxes");
        group.MapGet("{id:long}", GetById).WithName("GetStrongBoxById").WithSummary("Get strong box by ID");
        group.MapGet("by-eds/{idEds:int}", GetByEds).WithName("GetStrongBoxByEds").WithSummary("Get strong box by EDS ID");
        group.MapGet("current-balance", GetCurrentBalance).WithName("GetStrongBoxCurrentBalance").WithSummary("Get current balance");
        group.MapPost("", Create).WithName("CreateStrongBox").WithSummary("Create new strong box");

        return app;
    }

    private static async Task<IResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, IMediator mediator = null!)
    {
        var data = await mediator.Send(new StrongBoxGetList(pageNumber, pageSize));
        return data is null
            ? TypedResults.Json(ResponseApiService.Response(StatusCodes.Status404NotFound), statusCode: StatusCodes.Status404NotFound)
            : TypedResults.Json(ResponseApiService.Response(StatusCodes.Status200OK, data), statusCode: StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetById([FromRoute] long id, IMediator mediator)
    {
        var result = await mediator.Send(new StrongBoxGetId(id));
        return result == null
            ? TypedResults.NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, "StrongBox not found"))
            : TypedResults.Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
    }

    private static async Task<IResult> GetByEds([FromRoute] int idEds, IMediator mediator)
    {
        var result = await mediator.Send(new StrongBoxGetByEds(idEds));
        return result == null
            ? TypedResults.NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, "StrongBox not found for EDS"))
            : TypedResults.Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
    }

    private static async Task<IResult> GetCurrentBalance(IMediator mediator)
    {
        var result = await mediator.Send(new StrongBoxGetTotalBalance());
        return TypedResults.Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
    }

    private static async Task<IResult> Create([FromBody] StrongBoxCreateCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result == null
            ? TypedResults.BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, "Failed to create StrongBox"))
            : TypedResults.Created($"/api/v1/strong-box", ResponseApiService.Response(StatusCodes.Status201Created, result));
    }
}
