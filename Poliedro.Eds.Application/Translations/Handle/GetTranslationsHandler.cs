using MediatR;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Translations.Dtos;
using Poliedro.Eds.Application.Translations.Querys;


namespace Poliedro.Eds.Application.Translations.Handle;

public class GetTranslationsHandler(
    IRedisService redisService,
    ITolgeeService tolgeeService)
    : IRequestHandler<GetTranslationsQuery, TranslationsAvailableDto>
{
    private const string RedisPrefix = "translations";

    private static readonly Dictionary<string, string> FlagToCountryCodeMap = new()
    {
        { "🇺🇸", "US" },
        { "🇨🇴", "CO" }
    };

    public async Task<TranslationsAvailableDto> Handle(
        GetTranslationsQuery request,
        CancellationToken cancellationToken)
    {
        var translationKeys = await redisService.GetKeysByPatternAsync($"{RedisPrefix}:*");

        // Si no hay claves, buscar en Tolgee y cachear
        if (translationKeys.Count == 0)
        {
            var tolgeeTranslations = await tolgeeService.GetAllTranslationsFromTolgee();

            foreach (var (lang, dict) in tolgeeTranslations)
                await redisService.SetCacheAsync($"{RedisPrefix}:{lang}", dict, TimeSpan.FromHours(24));

            translationKeys = tolgeeTranslations.Keys.Select(lang => $"{RedisPrefix}:{lang}").ToList();
        }

        var translationsByLanguage = new Dictionary<string, Dictionary<string, string>>();
        var languages = new List<Language>();

        foreach (var key in translationKeys)
        {
            if (!key.StartsWith(RedisPrefix + ":")) continue;

            var langCode = key[(RedisPrefix.Length + 1)..];
            var translations = await redisService.GetCacheAsync<Dictionary<string, string>>(key);

            if (translations is null || translations.Count == 0) continue;

            translationsByLanguage[langCode] = translations;

            languages.Add(new Language
            {
                Code = langCode,
                Name = GetLanguageName(langCode),
                FlagEmoji = GetFlagEmoji(langCode)
            });
        }

        MapCountryCodes(languages);

        return new TranslationsAvailableDto(
            Translations: translationsByLanguage,
            Languages: languages
        );
    }

    private string GetLanguageName(string langCode) => langCode switch
    {
        "en" => "English",
        "es-CO" => "Español (Colombia)",
        _ => langCode
    };

    private string GetFlagEmoji(string langCode) => langCode switch
    {
        "en" => "🇺🇸",
        "es-CO" => "🇨🇴",
        _ => "🌐"
    };

    private void MapCountryCodes(IEnumerable<Language> languages)
    {
        foreach (var language in languages)
        {
            foreach (var flag in FlagToCountryCodeMap)
            {
                if (language.FlagEmoji.Contains(flag.Key))
                {
                    language.Code = flag.Value;
                    break;
                }
            }
        }
    }
}
