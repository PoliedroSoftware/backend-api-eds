using System.Text.Json.Serialization;

namespace Poliedro.Eds.Application.AWS.Configurations.Dto.Plemsi;

public class ApiPlemsiDto
{
    [JsonPropertyName("ApiKey")]
    public string ApiKey { get; set; } = string.Empty;
    [JsonPropertyName("PosUrl")]
    public string PosUrl { get; set; } = string.Empty;
    [JsonPropertyName("ApiUrl")]
    public string ApiUrl { get; set; } = string.Empty;
}
