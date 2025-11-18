using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Business.Commands.CreateBusiness;
using Poliedro.Eds.Application.Business.Commands.UpdateBusiness;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Application.Business.Queries.GellAllBusiness;
using Poliedro.Eds.Application.Business.Queries.GetBusinessById;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Business.Exepction;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class BusinessEndpoints
{
    public static IEndpointRouteBuilder MapBusinessEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/business")
            .WithTags("Business");

        group.MapGet("", GetAll)
            .WithName("GetAllBusinesses")
            .RequireAuthorization("AdminOrIslander")
            .Produces<BusinessDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapDelete("", Delete)
            .WithName("DeleteBusiness")
            .RequireAuthorization("AdminOnly")
            .Produces<BusinessDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("{id:int}", GetById)
            .WithName("GetBusinessById")
            .RequireAuthorization("AdminOrIslander")
            .WithSummary("Get business")
            .Produces<BusinessDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapPost("", Create)
            .WithName("CreateBusiness")
            .RequireAuthorization("AdminOrIslander")
            .WithSummary("Create new Business")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapPut("", Update)
            .WithName("UpdateBusiness")
            .RequireAuthorization("AdminOnly")
            .WithSummary("Update an existing Business")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> GetAll(
        [AsParameters] PaginationParams paginationParams,
        IMediator mediator)
    {
        var data = await mediator.Send(new GellAllBusinessQuery(new PaginationParams 
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

    private static async Task<IResult> Delete(
        [AsParameters] PaginationParams paginationParams,
        IMediator mediator)
    {
        var data = await mediator.Send(new GellAllBusinessQuery(new PaginationParams 
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
        var getBusinessQuery = new GetBusinessByIdQuery(Id: id);
        var result = await mediator.Send(getBusinessQuery);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    private static async Task<IResult> Create(
        [FromBody] CreateBusinessCommand createBusinessCommand,
        IMediator mediator)
    {
        var result = await mediator.Send(createBusinessCommand);

        return result.Match(
            onSuccess => TypedResults.Created(),
            onFailure => result.Error.HttpStatusCode == HttpStatusCode.Conflict
                ? TypedResults.Conflict(new
                {
                    status = 409,
                    type = result.Error.Code,
                    detail = result.Error.Description
                })
                : throw new Exception(result.Error.Description ?? "Unexpected error")
        );
    }

    private static async Task<IResult> Update(
        [FromBody] UpdateBusinessCommand updateBusinessCommand,
        IMediator mediator)
    {
        try
        {
            var result = await mediator.Send(updateBusinessCommand);

            if (!result.IsSuccess)
            {
                var errorMessage = result.Error?.Description ?? "Unknown error";
                var errorType = result.Error?.GetType().Name;

                return TypedResults.Json(new
                {
                    status = 500,
                    type = errorType,
                    message = errorMessage
                }, statusCode: StatusCodes.Status500InternalServerError);
            }

            return TypedResults.NoContent();
        }
        catch (BusinessDomainException ex)
        {
            return TypedResults.BadRequest(new
            {
                status = 400,
                type = "BusinessDomainException",
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return TypedResults.Json(new
            {
                status = 500,
                type = "UnhandledException",
                message = ex.Message
            }, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
