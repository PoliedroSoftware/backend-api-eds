using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Islander.Commands.CreateIslander;
using Poliedro.Eds.Application.Islander.Commands.UpdateIslander;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Application.Islander.Queries.GellAllIslander;
using Poliedro.Eds.Application.Islander.Queries.GetIslanderById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class IslanderEndpoints
{
    public static IEndpointRouteBuilder MapIslanderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/islander")
            .WithTags("Islander");

        group.MapGet("", GetAll)
            .WithName("GetAllIslanders")
            .WithSummary("Get all Islander entries")
            .RequireAuthorization("AdminOrIslander")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetIslanderById")
            .WithSummary("Get Islander by ID")
            .RequireAuthorization("AdminOrIslander")
            .Produces<IslanderDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("", Create)
            .WithName("CreateIslander")
            .WithSummary("Create new Islander")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("", Update)
            .WithName("UpdateIslander")
            .WithSummary("Update an existing Islander")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> GetAll(
        [AsParameters] PaginationParams paginationParams,
        IMediator mediator)
    {
        var data = await mediator.Send(new GellAllIslanderQuery(new PaginationParams 
        { 
            PageNumber = paginationParams.PageNumber, 
            PageSize = paginationParams.PageSize 
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
        var query = new GetIslanderByIdQuery(Id: id);
        var result = await mediator.Send(query);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Create(
        [FromBody] CreateIslanderCommand command,
        IMediator mediator)
    {
        return TypedResults.Ok(await mediator.Send(command));
    }

    private static async Task<IResult> Update(
        [FromBody] UpdateIslanderCommand command,
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
