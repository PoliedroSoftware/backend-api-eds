using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;

namespace Poliedro.Tolgee.Translations;

public class TranslationCachingService(
    ILogger<TranslationCachingService> logger,
    IServiceProvider serviceProvider,
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
        logger.LogInformation("Iniciando servicio de cache de traducciones...");

        using var scope = serviceProvider.CreateScope();
        var tolgeeService = scope.ServiceProvider.GetRequiredService<ITolgeeService>();

        try
        {
            if (await IsCachePopulated())
            {
                logger.LogInformation("Cache ya poblada.");
                return;
            }

            var translationsByLanguage = await tolgeeService.GetAllTranslationsFromTolgee();

            foreach (var (lang, translations) in translationsByLanguage)
            {
                var cacheKey = GetCacheKey(lang);
                await redisService.SetCacheAsync(cacheKey, translations, CacheExpiration);
                memoryCache.Set(cacheKey, translations, _memoryCacheOptions);
                logger.LogDebug("Cacheado {lang}", lang);
            }

            await redisService.SetCacheAsync(CacheStatusKey, true, CacheExpiration);
            memoryCache.Set(CacheStatusKey, true, _memoryCacheOptions);
            logger.LogInformation("Traducciones cacheadas.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error cacheando traducciones");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Deteniendo servicio de cache de traducciones.");
        return Task.CompletedTask;
    }

    public async Task<string?> GetTranslation(string language, string key)
    {
        var cacheKey = GetCacheKey(language);

        if (memoryCache.TryGetValue<Dictionary<string, string>>(cacheKey, out var memoryTranslations) &&
            memoryTranslations.TryGetValue(key, out var value))
        {
            return value;
        }

        var redisTranslations = await redisService.GetCacheAsync<Dictionary<string, string>>(cacheKey);
        if (redisTranslations != null && redisTranslations.TryGetValue(key, out var redisValue))
        {
            memoryCache.Set(cacheKey, redisTranslations, _memoryCacheOptions);
            return redisValue;
        }

        logger.LogWarning("Traducción no encontrada: {Language}/{Key}", language, key);
        return null;
    }

    public async Task<Dictionary<string, string>?> GetAllTranslations(string language)
    {
        var cacheKey = GetCacheKey(language);

        if (memoryCache.TryGetValue<Dictionary<string, string>>(cacheKey, out var memoryTranslations))
            return memoryTranslations;

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
        if (memoryCache.TryGetValue(CacheStatusKey, out bool memoryStatus) && memoryStatus)
            return true;

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
