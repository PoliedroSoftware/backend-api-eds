using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Phone.Commands.CreatePhone;
using Poliedro.Eds.Application.Phone.Commands.UpdatePhone;
using Poliedro.Eds.Application.Phone.Queries.GetAllPhones;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class PhoneEndpoints
{
    public static IEndpointRouteBuilder MapPhoneEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/phone")
            .WithTags("Phone")
            .RequireAuthorization("AdminOrIslander");

        group.MapGet("", GetAll).WithName("GetAllPhones").WithSummary("Get all phones");
        group.MapPost("", Create).WithName("CreatePhone").WithSummary("Create new phone");
        group.MapPut("", Update).WithName("UpdatePhone").WithSummary("Update existing phone");

        return app;
    }

    private static async Task<IResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, IMediator mediator = null!)
    {
        var data = await mediator.Send(new GetAllPhonesQuery(new PaginationParams 
        { PageNumber = pageNumber, PageSize = pageSize }));
        return data is null
            ? TypedResults.Json(ResponseApiService.Response(StatusCodes.Status404NotFound), statusCode: StatusCodes.Status404NotFound)
            : TypedResults.Json(ResponseApiService.Response(StatusCodes.Status200OK, data), statusCode: StatusCodes.Status200OK);
    }

    private static async Task<IResult> Create([FromBody] CreatePhoneCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.Match(
            onSuccess => TypedResults.Created($"/api/v1/phone", result.Value),
            onFailure => TypedResults.BadRequest(onFailure));
    }

    private static async Task<IResult> Update([FromBody] UpdatePhoneCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return !result.IsSuccess
            ? TypedResults.Json(ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error), statusCode: StatusCodes.Status500InternalServerError)
            : TypedResults.NoContent();
    }
}
