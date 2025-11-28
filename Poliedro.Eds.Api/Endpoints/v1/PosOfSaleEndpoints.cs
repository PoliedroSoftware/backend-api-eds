using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.PosOfSale.Commands.CreatePosOfSale;
using Poliedro.Eds.Application.PosOfSale.Commands.UpdatePosOfSale;
using Poliedro.Eds.Application.PosOfSale.Queries.GetAllPosOfSale;
using Poliedro.Eds.Application.PosOfSale.Queries.GetPostOfSaleById;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class PosOfSaleEndpoints
{
    public static IEndpointRouteBuilder MapPosOfSaleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/pos-of-sale")
            .WithTags("PosOfSale")
            .RequireAuthorization("AdminOrIslander");

        group.MapGet("", GetAll)
            .WithName("GetAllPosOfSale")
            .WithSummary("Get all Pos Of Sale entries")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetPosOfSaleById")
            .WithSummary("Get Pos Of Sale by ID")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("", Create)
            .WithName("CreatePosOfSale")
            .WithSummary("Create new Pos Of Sale")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("", Update)
            .WithName("UpdatePosOfSale")
            .WithSummary("Update an existing Pos Of Sale")
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
        var result = await mediator.Send(new GellAllPosOfSaleQuery(new PaginationParams
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        }));

        return result.Match(
            onSuccess => TypedResults.Json(
                ResponseApiService.Response(StatusCodes.Status200OK, result.Value),
                statusCode: StatusCodes.Status200OK),
            onFailure => TypedResults.Json(
                ResponseApiService.Response((int)result.Error!.HttpStatusCode, result.Error),
                statusCode: (int)result.Error!.HttpStatusCode)
        );
    }

    private static async Task<IResult> GetById(
        [FromRoute] int id,
        IMediator mediator)
    {
        var query = new GetPosOfSaleByIdQuery(Id: id);
        var result = await mediator.Send(query);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Create(
        [FromBody] CreatePosOfSaleCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created());
    }

    private static async Task<IResult> Update(
        [FromBody] UpdatePosOfSaleCommand command,
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
