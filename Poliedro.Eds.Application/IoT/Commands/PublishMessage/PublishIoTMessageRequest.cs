namespace Poliedro.Eds.Application.IoT.Commands.PublishMessage;


public record PublishIoTMessageRequest
{
    public IoTMessageDto Message { get; init; } = null!;
    public string? Topic { get; init; }
}


public record IoTMessageDto
{
   
    public string Input1 { get; init; } = string.Empty;
    public string Input2 { get; init; } = string.Empty;
    public string Output1 { get; init; } = string.Empty;
    public string Output2 { get; init; } = string.Empty;
}
