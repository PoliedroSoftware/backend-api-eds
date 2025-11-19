using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.DispenserType.Commands.CreateDispenserType;
using Poliedro.Eds.Application.DispenserType.Commands.UpdateDispenserType;
using Poliedro.Eds.Application.DispenserType.Dtos;
using Poliedro.Eds.Application.DispenserType.Queries.GellAllDispenserType;
using Poliedro.Eds.Application.DispenserType.Queries.GetDispenserTypeById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class DispenserTypeEndpoints
{
    public static IEndpointRouteBuilder MapDispenserTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/dispenser-type")
            .WithTags("DispenserType");

        group.MapGet("", GetAll)
            .WithName("GetAllDispenserTypes")
            .WithSummary("Get all DispenserType entries")
            .RequireAuthorization("AdminOrIslander")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetDispenserTypeById")
            .WithSummary("Get DispenserType by ID")
            .RequireAuthorization("AdminOrIslander")
            .Produces<DispenserTypeDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("", Create)
            .WithName("CreateDispenserType")
            .WithSummary("Create new DispenserType")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("", Update)
            .WithName("UpdateDispenserType")
            .WithSummary("Update an existing DispenserType")
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
        var data = await mediator.Send(new GellAllDispenserTypeQuery(new PaginationParams 
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
        var query = new GetDispenserTypeByIdQuery(Id: id);
        var result = await mediator.Send(query);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Create(
        [FromBody] CreateDispenserTypeCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created(),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Update(
        [FromBody] UpdateDispenserTypeCommand command,
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
