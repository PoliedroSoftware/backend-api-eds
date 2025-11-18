using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.IoT.Commands.PublishMessage;
using Poliedro.Eds.Domain.IoT.Models;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class IoTEndpoints
{
    public static IEndpointRouteBuilder MapIoTEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/iot")
            .WithTags("IoT")
            .RequireAuthorization("AdminOrIslander");

        group.MapPost("publish", PublishMessage)
            .WithName("PublishIoTMessage")
            .WithSummary("Publish message to AWS IoT Core")
            .WithDescription("Publica un mensaje MQTT en AWS IoT Core para comunicación con dispositivos IoT")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> PublishMessage(
        [FromBody] PublishIoTMessageRequest request,
        IMediator mediator)
    {
        var iotMessage = new IoTMessage
        {
            Input1 = request.Message.Input1,
            Input2 = request.Message.Input2,
            Output1 = request.Message.Output1,
            Output2 = request.Message.Output2
        };
        var topic = string.IsNullOrWhiteSpace(request.Topic) ? null : request.Topic;
        var command = new PublishIoTMessageCommand(iotMessage, topic);
        var result = await mediator.Send(command);

        if (!result)
        {
            return TypedResults.Json(
                ResponseApiService.Response(
                    StatusCodes.Status500InternalServerError, 
                    "Failed to publish message to IoT Core"
                ),
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        return TypedResults.Ok(
            ResponseApiService.Response(StatusCodes.Status200OK, new { 
                Success = result,
                Message = "IoT message published successfully",
                Topic = topic ?? "default"
            })
        );
    }
}

public record PublishIoTMessageRequest(IoTMessage Message, string Topic);
