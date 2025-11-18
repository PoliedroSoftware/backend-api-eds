using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.TransferValidation.Commands.CreateTransferValidation;
using Poliedro.Eds.Application.TransferValidation.Commands.UpdateTransferValidation;
using Poliedro.Eds.Application.Common.Features;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class TransferValidationEndpoints
{
    public static IEndpointRouteBuilder MapTransferValidationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/transfer-validation")
            .WithTags("TransferValidation")
            .RequireAuthorization("AdminOrIslander");

        group.MapPost("", Create).WithName("CreateTransferValidation").WithSummary("Create new transfer validation");
        group.MapPut("", Update).WithName("UpdateTransferValidation").WithSummary("Update transfer validation");

        return app;
    }

    private static async Task<IResult> Create([FromBody] CreateTransferValidationCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result == null
            ? TypedResults.BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, "Failed to create transfer validation"))
            : TypedResults.Created($"/api/v1/transfer-validation", ResponseApiService.Response(StatusCodes.Status201Created, result));
    }

    private static async Task<IResult> Update([FromBody] UpdateTransferValidationCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result == null
            ? TypedResults.BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, "Failed to update transfer validation"))
            : TypedResults.NoContent();
    }
}
