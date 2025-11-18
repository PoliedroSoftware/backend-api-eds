using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poliedro.Eds.Api.Common.Extensions;
using Poliedro.Eds.Application.OpenAI.Commands.SendMessage;
using Poliedro.Eds.Application.OpenAI.Dtos;
using Poliedro.Eds.Application.OpenAI.Queries.GetChatHistory;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class OpenAIEndpoints
{
    public static IEndpointRouteBuilder MapOpenAIEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/openai")
            .WithTags("OpenAI")
            .RequireAuthorization("AdminOrIslander");

        group.MapPost("send-message", SendMessage).WithName("SendOpenAIMessage").WithSummary("Send message to OpenAI");
        group.MapGet("chat-history", GetChatHistory).WithName("GetChatHistory").WithSummary("Get chat history");

        return app;
    }

    private static async Task<IResult> SendMessage([FromBody] OpenAIRequestDto request, HttpContext httpContext, IMediator mediator)
    {
        var userId = httpContext.User.FindFirst("sub")?.Value ?? 
                    httpContext.User.FindFirst("name")?.Value ?? 
                    "anonymous";

        var command = new SendOpenAIMessageCommand(request, userId);
        var result = await mediator.Send(command);

        return result.Match(
            onSuccess => TypedResults.Ok(result.Value),
            onFailure => TypedResults.BadRequest(onFailure));
    }

    private static async Task<IResult> GetChatHistory([FromQuery] int pageNumber, [FromQuery] int pageSize, HttpContext httpContext, IMediator mediator)
    {
        var userId = httpContext.User.FindFirst("sub")?.Value ?? 
                    httpContext.User.FindFirst("name")?.Value ??
                    "anonymous";

        var result = await mediator.Send(new GetChatHistoryQuery(userId, pageNumber, pageSize));
        return TypedResults.Ok(result);
    }
}
