using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Eds.Application.Translations.Dtos;

public record TranslationsAvailableDto(
Dictionary<string, Dictionary<string, string>> Translations,
List<Language> Languages);
