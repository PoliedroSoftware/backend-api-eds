using System.Text.Json;
using Poliedro.Eds.Application.Ports.Translations;


namespace Poliedro.Tolgee.Translations;

public class TolgeeService(
    IHttpClientFactory httpClientFactory) : ITolgeeService
{
    public async Task<Dictionary<string, Dictionary<string, string>>> GetAllTranslationsFromTolgee()
    {
        var httpClient = httpClientFactory.CreateClient(nameof(TolgeeService));
        var response = await httpClient.GetAsync("translations?size=1000");

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Error al consultar Tolgee Transtalations: {response.StatusCode}");

        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<RootObject>(body)
                   ?? throw new Exception("Error deserializando respuesta de Tolgee");

        var result = new Dictionary<string, Dictionary<string, string>>();

        foreach (var key in data.translations.Keys)
        {
            foreach (var translation in key.Translations)
            {
                var lang = translation.Key;
                var text = translation.Value.Text;

                if (!result.TryGetValue(lang, out var dict))
                {
                    dict = new Dictionary<string, string>();
                    result[lang] = dict;
                }

                dict[key.KeyName] = text;
            }
        }

        return result;
    }
}
