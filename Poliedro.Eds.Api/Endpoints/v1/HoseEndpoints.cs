using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Hose.Commands.CreateHose;
using Poliedro.Eds.Application.Hose.Commands.UpdateHose;
using Poliedro.Eds.Application.Hose.Dtos;
using Poliedro.Eds.Application.Hose.Queries.GellAllHose;
using Poliedro.Eds.Application.Hose.Queries.GetHoseById;
using Poliedro.Eds.Application.Hose.Queries.GetLastAccumulated;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class HoseEndpoints
{
    public static IEndpointRouteBuilder MapHoseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/hose")
            .WithTags("Hose")
            .RequireAuthorization("AdminOrIslander");

        group.MapGet("", GetAll)
            .WithName("GetAllHoses")
            .WithSummary("Get all Hose entries")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetHoseById")
            .WithSummary("Get Hose by ID")
            .Produces<HoseDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("last-accumulated", GetLastAccumulated)
            .WithName("GetLastAccumulatedHose")
            .WithSummary("Get last accumulated value for a hose")
            .Produces(StatusCodes.Status200OK);

        group.MapPost("", Create)
            .WithName("CreateHose")
            .WithSummary("Create new Hose")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("", Update)
            .WithName("UpdateHose")
            .WithSummary("Update an existing Hose")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> GetAll(
        [AsParameters] PaginationParams paginationParams,
        IMediator mediator)
    {
        var data = await mediator.Send(new GellAllHoseQuery(new PaginationParams 
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
        var query = new GetHoseByIdQuery(Id: id);
        var result = await mediator.Send(query);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> GetLastAccumulated(
        [FromQuery] int idDispenser,
        [FromQuery] int idHose,
        IMediator mediator)
    {
        var query = new GetLastAccumulatedQuery(idDispenser, idHose);
        var result = await mediator.Send(query);
        
        if (result == null)
        {
            return TypedResults.NotFound(
                ResponseApiService.Response(StatusCodes.Status404NotFound, "Last accumulated not found"));
        }
        
        return TypedResults.Ok(ResponseApiService.Response(StatusCodes.Status200OK, result));
    }

    private static async Task<IResult> Create(
        [FromBody] CreateHoseCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created($"/api/v1/hose/{result.Value!.IdHose}", result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Update(
        [FromBody] UpdateHoseCommand command,
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
