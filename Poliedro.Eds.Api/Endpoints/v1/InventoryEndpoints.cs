using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Inventory.Queries.GetInventoryList;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class InventoryEndpoints
{
    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/inventory")
            .WithTags("Inventory")
            .RequireAuthorization("AdminOrIslander");

        group.MapGet("", GetInventoryAll).WithName("GetInventoryAll").WithSummary("Get all inventory items");

        return app;
    }

    private static async Task<IResult> GetInventoryAll([AsParameters] PaginationParams paginationParams, IMediator mediator)
    {
        var data = await mediator.Send(new GetInventoriesListQuery(new PaginationParams 
        { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize }));
        return data is null
            ? TypedResults.Json(ResponseApiService.Response(StatusCodes.Status404NotFound), statusCode: StatusCodes.Status404NotFound)
            : TypedResults.Json(ResponseApiService.Response(StatusCodes.Status200OK, data), statusCode: StatusCodes.Status200OK);
    }
}
