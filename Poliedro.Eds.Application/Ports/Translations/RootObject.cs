using System.Text.Json.Serialization;

namespace Poliedro.Eds.Application.Ports.Translations
{
    public class RootObject
    {
        [JsonPropertyName("_embedded")]
        public Embedded translations { get; set; }

        [JsonPropertyName("selectedLanguages")]
        public List<Language> SelectedLanguages { get; set; }

        public class Embedded
        {
            [JsonPropertyName("keys")]
            public List<Translation> Keys { get; set; }
        }
    }

}
