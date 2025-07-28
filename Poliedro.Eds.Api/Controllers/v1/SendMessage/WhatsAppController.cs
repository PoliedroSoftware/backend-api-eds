using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/whatsapp")]
public class WhatsAppController : ControllerBase
{
    private readonly IMediator _mediator;

    public WhatsAppController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var command = new SendWhatsAppMessageCommand(request.PhoneNumber, request.Message);
        await _mediator.Send(command);

        return Ok("Message sent successfully.");
    }
}

public class SendMessageRequest
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
}
