using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory;
using Poliedro.Eds.Application.ShoppingProductInventory.Queries.GellAllShoppingProductInventory;
using Poliedro.Eds.Application.ShoppingProductInventory.Queries.GetShoppingProductInventoryById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class ShoppingProductInventoryEndpoints
{
    public static IEndpointRouteBuilder MapShoppingProductInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/shopping-product-inventory")
            .WithTags("ShoppingProductInventory")
            .RequireAuthorization("AdminOrIslander");

        group.MapGet("", GetAll).WithName("GetAllShoppingProductInventories").WithSummary("Get all shopping product inventories");
        group.MapPost("", Create).WithName("CreateShoppingProductInventory").WithSummary("Create new shopping product inventory");
        group.MapGet("by-shopping/{id:int}", GetByIdShopping).WithName("GetShoppingProductInventoryByIdShopping").WithSummary("Get by shopping ID");

        return app;
    }

    private static async Task<IResult> GetAll([AsParameters] PaginationParams paginationParams, IMediator mediator)
    {
        var data = await mediator.Send(new GellAllShoppingProductInventoryQuery(new PaginationParams 
        { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        return data is null
            ? TypedResults.Json(ResponseApiService.Response(StatusCodes.Status404NotFound), statusCode: StatusCodes.Status404NotFound)
            : TypedResults.Json(ResponseApiService.Response(StatusCodes.Status200OK, data), statusCode: StatusCodes.Status200OK);
    }

    private static async Task<IResult> Create([FromBody] CreateShoppingProductInventoryCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created($"/api/v1/shopping-product-inventory", result.Value),
            onFailure => TypedResults.BadRequest(onFailure));
    }

    private static async Task<IResult> GetByIdShopping([FromRoute] int id, IMediator mediator)
    {
        var result = await mediator.Send(new GetShoppingProductInventoryByIdQuery(Id: id));
        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure));
    }
}
