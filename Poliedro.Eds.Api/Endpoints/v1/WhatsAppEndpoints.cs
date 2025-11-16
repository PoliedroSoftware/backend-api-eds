using MediatR;

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

    private static async Task<IResult> SendMessage(IMediator mediator)
    {
        await mediator.Send(new SendWhatsAppMessageCommand());
        return TypedResults.Ok(new { Message = "WhatsApp message sent successfully" });
    }
}
