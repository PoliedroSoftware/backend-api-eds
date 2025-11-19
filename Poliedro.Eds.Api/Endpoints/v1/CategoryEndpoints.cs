using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Category.Commands.CreateCategory;
using Poliedro.Eds.Application.Category.Commands.UpdateCategory;
using Poliedro.Eds.Application.Category.Dtos;
using Poliedro.Eds.Application.Category.Queries.GellAllCategory;
using Poliedro.Eds.Application.Category.Queries.GetCategoryById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/category")
            .WithTags("Category");

        group.MapGet("", GetAll)
            .WithName("GetAllCategories")
            .WithSummary("Get all Category entries")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetCategoryById")
            .WithSummary("Get Category by ID")
            .RequireAuthorization("AdminOnly")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("", Create)
            .WithName("CreateCategory")
            .WithSummary("Create new Category")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("", Update)
            .WithName("UpdateCategory")
            .WithSummary("Update an existing Category")
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
        var data = await mediator.Send(new GellAllCategoryQuery(new PaginationParams 
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
        var query = new GetCategoryByIdQuery(Id: id);
        var result = await mediator.Send(query);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Create(
        [FromBody] CreateCategoryCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created(),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Update(
        [FromBody] UpdateCategoryCommand command,
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
