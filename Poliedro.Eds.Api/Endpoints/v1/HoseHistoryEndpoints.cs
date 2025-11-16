using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.HoseHistory.Commands.CreateHoseHistory;
using Poliedro.Eds.Application.HoseHistory.Commands.UpdateHoseHistory;
using Poliedro.Eds.Application.HoseHistory.Dtos;
using Poliedro.Eds.Application.HoseHistory.Queries.GellAllHoseHistory;
using Poliedro.Eds.Application.HoseHistory.Queries.GetHoseHistoryById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class HoseHistoryEndpoints
{
    public static IEndpointRouteBuilder MapHoseHistoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/hose-history")
            .WithTags("HoseHistory")
            .RequireAuthorization("AdminOrIslander");

        group.MapGet("", GetAll)
            .WithName("GetAllHoseHistories")
            .WithSummary("Get all HoseHistory entries");

        group.MapGet("{id:int}", GetById)
            .WithName("GetHoseHistoryById")
            .WithSummary("Get HoseHistory by ID");

        group.MapPost("", Create)
            .WithName("CreateHoseHistory")
            .WithSummary("Create new HoseHistory");

        group.MapPut("", Update)
            .WithName("UpdateHoseHistory")
            .WithSummary("Update an existing HoseHistory");

        return app;
    }

    private static async Task<IResult> GetAll(
        [AsParameters] PaginationParams paginationParams,
        IMediator mediator)
    {
        var data = await mediator.Send(new GellAllHoseHistoryQuery(new PaginationParams 
        { 
            PageNumber = paginationParams.PageNumber, 
            PageSize = paginationParams.PageSize 
        }));
        
        return data is null
            ? TypedResults.Json(ResponseApiService.Response(StatusCodes.Status404NotFound), statusCode: StatusCodes.Status404NotFound)
            : TypedResults.Json(ResponseApiService.Response(StatusCodes.Status200OK, data), statusCode: StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetById([FromRoute] int id, IMediator mediator)
    {
        var result = await mediator.Send(new GetHoseHistoryByIdQuery(Id: id));
        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure));
    }

    private static async Task<IResult> Create([FromBody] CreateHoseHistoryCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created());
    }

    private static async Task<IResult> Update([FromBody] UpdateHoseHistoryCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return !result.IsSuccess
            ? TypedResults.Json(ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error), statusCode: StatusCodes.Status500InternalServerError)
            : TypedResults.NoContent();
    }
}
