using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.IoT.Commands.PublishMessage;
using Poliedro.Eds.Domain.IoT.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.IoT;

[Route("api/v1/iot")]
[ApiController]
public class IoTController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation(
        Summary = "Publish message to AWS IoT Core",
        Description = "Publica un mensaje MQTT en AWS IoT Core para comunicación con dispositivos IoT"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Message published successfully.", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpPost("publish")]
    public async Task<IActionResult> PublishMessage([FromBody] PublishIoTMessageRequest request)
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
            return StatusCode(
                StatusCodes.Status500InternalServerError, 
                ResponseApiService.Response(
                    StatusCodes.Status500InternalServerError, 
                    "Failed to publish message to IoT Core"
                )
            );
        }

        return StatusCode(
            StatusCodes.Status200OK, 
            ResponseApiService.Response(StatusCodes.Status200OK, new { 
                Success = result,
                Message = "IoT message published successfully",
                Topic = topic ?? "default"
            })
        );
    }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
}

public record PublishIoTMessageRequest(IoTMessage Message, string Topic);
