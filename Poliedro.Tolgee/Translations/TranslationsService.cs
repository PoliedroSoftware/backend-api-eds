using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using System.Text.Json;

namespace Poliedro.Tolgee.Translations;

public class TranslationCachingService(
    ILogger<TranslationCachingService> logger,
    IHttpClientFactory httpClientFactory,
    IRedisService redisService,
    IMemoryCache memoryCache) : IHostedService
{
    private const string RedisPrefix = "translations";
    private const string CacheStatusKey = "translations:cache_status";
    private readonly TimeSpan CacheExpiration = TimeSpan.FromHours(24);
    private readonly MemoryCacheEntryOptions _memoryCacheOptions = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(30),
        Priority = CacheItemPriority.High
    };

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Translation Caching Service...");
        try
        {
            if (await IsCachePopulated())
            {
                logger.LogInformation("Translations cache is already populated.");
                return;
            }

            var httpClient = httpClientFactory.CreateClient(nameof(TranslationCachingService));
            var response = await httpClient.GetAsync("translations?size=1000");

            if (!response.IsSuccessStatusCode) throw new Exception($"Error fetching translations: {response.StatusCode}");
            

            var responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<RootObject>(responseBody)
                ?? throw new Exception("Error deserializing translations data");

            var translationsByLanguage = new Dictionary<string, Dictionary<string, string>>();

            foreach (var key in data.translations.Keys)
            {
                foreach (var translation in key.Translations)
                {
                    var language = translation.Key;
                    var translationDetail = translation.Value;

                    if (!translationsByLanguage.TryGetValue(language, out var langDict))
                    {
                        langDict = new Dictionary<string, string>();
                        translationsByLanguage[language] = langDict;
                    }

                    langDict[key.KeyName] = translationDetail.Text;
                }
            }

           
            foreach (var (language, translations) in translationsByLanguage)
            {
                var cacheKey = GetCacheKey(language);
                await redisService.SetCacheAsync(cacheKey, translations, CacheExpiration);
                memoryCache.Set(cacheKey, translations, _memoryCacheOptions);
                logger.LogDebug("Cached translations for {Language}", language);
            }

            await redisService.SetCacheAsync(CacheStatusKey, true, CacheExpiration);
            logger.LogInformation("Translations cached for {LanguageCount} languages", translationsByLanguage.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during translation caching: {Message}", ex.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Translation Caching Service stopping.");
        return Task.CompletedTask;
    }

    public async Task<string?> GetTranslation(string language, string key)
    {
        var cacheKey = GetCacheKey(language);
        if (memoryCache.TryGetValue<Dictionary<string, string>>(cacheKey, out var memoryTranslations) &&
            memoryTranslations != null &&
            memoryTranslations.TryGetValue(key, out var memoryValue))
        {
            return memoryValue;
        }

        var redisTranslations = await redisService.GetCacheAsync<Dictionary<string, string>>(cacheKey);
        if (redisTranslations != null && redisTranslations.TryGetValue(key, out var redisValue))
        {
            memoryCache.Set(cacheKey, redisTranslations, _memoryCacheOptions);
            return redisValue;
        } 
        logger.LogWarning("Translation not found: {Language}/{Key}", language, key);
        return null;
    }

    public async Task<Dictionary<string, string>?> GetAllTranslations(string language)
    {
        var cacheKey = GetCacheKey(language);
        if (memoryCache.TryGetValue<Dictionary<string, string>>(cacheKey, out var memoryTranslations))
        {
            return memoryTranslations;
        }
        var redisTranslations = await redisService.GetCacheAsync<Dictionary<string, string>>(cacheKey);
        if (redisTranslations != null)
        {
            memoryCache.Set(cacheKey, redisTranslations, _memoryCacheOptions);
            return redisTranslations;
        }

        return null;
    }

    private async Task<bool> IsCachePopulated()
    {
        if (memoryCache.TryGetValue<bool>(CacheStatusKey, out var memoryStatus) && memoryStatus) return true;
        var redisStatus = await redisService.GetCacheAsync<bool>(CacheStatusKey);
        if (redisStatus)
        {
            memoryCache.Set(CacheStatusKey, true, _memoryCacheOptions);
            return true;
        }

        return false;
    }

    private static string GetCacheKey(string language) => $"{RedisPrefix}:{language}";
}