using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Island.Commands.CreateIsland;
using Poliedro.Eds.Application.Island.Commands.UpdateIsland;
using Poliedro.Eds.Application.Island.Dtos;
using Poliedro.Eds.Application.Island.Queries.GellAllIsland;
using Poliedro.Eds.Application.Island.Queries.GetIslandById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class IslandEndpoints
{
    public static IEndpointRouteBuilder MapIslandEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/island")
            .WithTags("Island");

        group.MapGet("", GetAll)
            .WithName("GetAllIslands")
            .WithSummary("Get all Island entries")
            .RequireAuthorization("AdminOrIslander")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetIslandById")
            .WithSummary("Get Island by ID")
            .RequireAuthorization("AdminOrIslander")
            .Produces<IslandDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("", Create)
            .WithName("CreateIsland")
            .WithSummary("Create new Island")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("", Update)
            .WithName("UpdateIsland")
            .WithSummary("Update an existing Island")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        IMediator mediator = null!)
    {
        var data = await mediator.Send(new GellAllIslandQuery(new PaginationParams 
        { 
            PageNumber = pageNumber, PageSize = pageSize 
        }));
        
        if (data is null)
        {
            return TypedResults.Json(
                ResponseApiService.Response(StatusCodes.Status404NotFound),
                statusCode: StatusCodes.Status404NotFound);
        }
        
        return TypedResults.Json(
            ResponseApiService.Response(StatusCodes.Status200OK, data),
            statusCode: StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetById(
        [FromRoute] int id,
        IMediator mediator)
    {
        var query = new GetIslandByIdQuery(Id: id);
        var result = await mediator.Send(query);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Create(
        [FromBody] CreateIslandCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created()
        );
    }

    private static async Task<IResult> Update(
        [FromBody] UpdateIslandCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
        {
            return TypedResults.Json(
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error),
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return TypedResults.NoContent();
    }
}
