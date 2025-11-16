using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.CourtDispensersInventory.Commands.CreateCourtDispensersInventory;
using Poliedro.Eds.Application.CourtDispensersInventory.Commands.UpdateCourtDispensersInventory;
using Poliedro.Eds.Application.CourtDispensersInventory.Dtos;
using Poliedro.Eds.Application.CourtDispensersInventory.Queries.GellAllCourtDispensersInventory;
using Poliedro.Eds.Application.CourtDispensersInventory.Queries.GetCourtDispensersInventoryById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class CourtDispensersInventoryEndpoints
{
    public static IEndpointRouteBuilder MapCourtDispensersInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/court-dispensers-inventory")
            .WithTags("CourtDispensersInventory");

        group.MapGet("", GetAll)
            .WithName("GetAllCourtDispensersInventorys")
            .WithSummary("Get all CourtDispensersInventory entries")
            .RequireAuthorization("AdminOrIslander")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetCourtDispensersInventoryById")
            .WithSummary("Get CourtDispensersInventory by ID")
            .RequireAuthorization("AdminOrIslander")
            .Produces<CourtDispensersInventoryDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("", Create)
            .WithName("CreateCourtDispensersInventory")
            .WithSummary("Create new CourtDispensersInventory")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("", Update)
            .WithName("UpdateCourtDispensersInventory")
            .WithSummary("Update an existing CourtDispensersInventory")
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
        var data = await mediator.Send(new GellAllCourtDispensersInventoryQuery(new PaginationParams 
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
        var query = new GetCourtDispensersInventoryByIdQuery(Id: id);
        var result = await mediator.Send(query);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Create(
        [FromBody] CreateCourtDispensersInventoryCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created($"/api/v1/court-dispensers-inventory/{result.Value!.IdCourtDispensersInventory}", result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Update(
        [FromBody] UpdateCourtDispensersInventoryCommand command,
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
