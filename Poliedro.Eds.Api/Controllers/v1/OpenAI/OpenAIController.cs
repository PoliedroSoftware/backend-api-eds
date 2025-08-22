using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.Common.Features;
using Poliedro.Eds.Application.OpenAI.Commands.SendMessage;
using Poliedro.Eds.Application.OpenAI.Dtos;
using Poliedro.Eds.Application.OpenAI.Queries.GetChatHistory;
using Swashbuckle.AspNetCore.Annotations;

namespace Poliedro.Eds.Api.Controllers.v1.OpenAI;

[Route("api/v1/openai")]
[ApiController]
public class OpenAIController(IMediator mediator) : ControllerBase
{
    [SwaggerOperation(
        Summary = "Send message to OpenAI and get response",
        Description = "Sends a message to OpenAI GPT models and returns the AI-generated response")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(OpenAIResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpPost("chat")]
    public async Task<IResult> SendMessage([FromBody] OpenAIRequestDto request)
    {
        var userId = HttpContext.User.FindFirst("sub")?.Value ?? 
                    HttpContext.User.FindFirst("name")?.Value ?? 
                    "anonymous";

        var command = new SendOpenAIMessageCommand(request, userId);
        var result = await mediator.Send(command);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure)
        );
    }

    [SwaggerOperation(
        Summary = "Get chat history for authenticated user",
        Description = "Retrieves the chat history (previous conversations) for the authenticated user")]
    [SwaggerResponse(StatusCodes.Status200OK, "The operation was successful.", typeof(IEnumerable<OpenAIResponseDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect request parameters.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "The request lacks valid authentication credentials.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error processing the request.", typeof(ProblemDetails))]
    [Authorize(Policy = "AdminOrIslander")]
    [HttpGet("history")]
    public async Task<IActionResult> GetChatHistory([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = HttpContext.User.FindFirst("sub")?.Value ?? 
                    HttpContext.User.FindFirst("name")?.Value ?? 
                    "anonymous";

        var query = new GetChatHistoryQuery(userId, pageNumber, pageSize);
        var result = await mediator.Send(query);

        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ResponseApiService.Response(StatusCodes.Status500InternalServerError, result.Error));
        }

        return StatusCode(StatusCodes.Status200OK, 
            ResponseApiService.Response(StatusCodes.Status200OK, result.Value));
    }
}