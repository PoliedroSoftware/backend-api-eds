using System.Text.Json.Serialization;

namespace Poliedro.Eds.Domain.IoT.Models;

public class IoTMessage
{
    [JsonPropertyName("input1")]
    public string Input1 { get; set; } = string.Empty;

    [JsonPropertyName("input2")]
    public string Input2 { get; set; } = string.Empty;

    [JsonPropertyName("output1")]
    public string Output1 { get; set; } = string.Empty;

    [JsonPropertyName("output2")]
    public string Output2 { get; set; } = string.Empty;
}
