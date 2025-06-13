namespace Poliedro.Eds.Application.Ports.Translations;

public interface ITranslationService
{
    Task<bool> IsCachePopulated();
    Task<string?> GetTranslationByKey(string key);
}
