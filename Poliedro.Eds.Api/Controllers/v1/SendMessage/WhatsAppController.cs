using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/whatsapp")]
public class WhatsAppController (IMediator mediator) : ControllerBase
{

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage()
    {
        var command = new SendWhatsAppMessageCommand();
        await mediator.Send(command);

        return Ok("Message sent successfully.");
    }
}
