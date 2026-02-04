using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Poliedro.Eds.Api.Endpoints.v1;

public static class WhatsAppEndpoints
{
    public static IEndpointRouteBuilder MapWhatsAppEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/send-message")
            .WithTags("WhatsApp")
            .RequireAuthorization("AdminOrIslander");

        group.MapPost("", SendMessage).WithName("SendWhatsAppMessage").WithSummary("Send WhatsApp message");

        return app;
    }

    private static async Task<IResult> SendMessage(
        [FromBody] SendWhatsAppMessageCommand command,
        IMediator mediator
        )
    {
        await mediator.Send(command);
        return TypedResults.Ok(new { Message = "WhatsApp message sent successfully" });
    }
}
