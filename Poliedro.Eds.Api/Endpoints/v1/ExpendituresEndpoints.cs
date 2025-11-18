using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Expenditures.Commands.CreateExpenditures;
using Poliedro.Eds.Application.Expenditures.Commands.UpdateExpenditures;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Application.Expenditures.Queries.GellAllExpenditures;
using Poliedro.Eds.Application.Expenditures.Queries.GetExpendituresById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class ExpendituresEndpoints
{
    public static IEndpointRouteBuilder MapExpendituresEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/expenditures")
            .WithTags("Expenditures");

        group.MapGet("", GetAll)
            .WithName("GetAllExpendituress")
            .WithSummary("Get all Expenditures entries")
            .RequireAuthorization("AdminOrIslander")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetExpendituresById")
            .WithSummary("Get Expenditures by ID")
            .RequireAuthorization("AdminOrIslander")
            .Produces<ExpendituresDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("", Create)
            .WithName("CreateExpenditures")
            .WithSummary("Create new Expenditures")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("", Update)
            .WithName("UpdateExpenditures")
            .WithSummary("Update an existing Expenditures")
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
        var data = await mediator.Send(new GellAllExpendituresQuery(new PaginationParams 
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
        var query = new GetExpendituresByIdQuery(Id: id);
        var result = await mediator.Send(query);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Create(
        [FromBody] CreateExpendituresCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created(),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Update(
        [FromBody] UpdateExpendituresCommand command,
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
