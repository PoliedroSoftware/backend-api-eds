using System.Text.Json.Serialization;

namespace Poliedro.Eds.Application.Ports.Translations;

public class TranslationDetail
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
