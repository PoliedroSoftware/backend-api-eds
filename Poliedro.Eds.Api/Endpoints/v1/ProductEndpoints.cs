using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Product.Commands.CreateProduct;
using Poliedro.Eds.Application.Product.Commands.UpdateProduct;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Application.Product.Queries.GellAllProduct;
using Poliedro.Eds.Application.Product.Queries.GetProductById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/product")
            .WithTags("Product");

        group.MapGet("", GetAll)
            .WithName("GetAllProducts")
            .WithSummary("Get all Product entries")
            .RequireAuthorization("AdminOrIslander")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetProductById")
            .WithSummary("Get Product by ID")
            .RequireAuthorization("AdminOrIslander")
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("", Create)
            .WithName("CreateProduct")
            .WithSummary("Create new Product")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("", Update)
            .WithName("UpdateProduct")
            .WithSummary("Update an existing Product")
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
        var data = await mediator.Send(new GellAllProductQuery(new PaginationParams 
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
        var query = new GetProductByIdQuery(Id: id);
        var result = await mediator.Send(query);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Create(
        [FromBody] CreateProductCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(onSuccess => TypedResults.Created());
    }

    private static async Task<IResult> Update(
        [FromBody] UpdateProductCommand command,
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
