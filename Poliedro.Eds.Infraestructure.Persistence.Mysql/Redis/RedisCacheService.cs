using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;
using StackExchange.Redis;
using System.Text.Json;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Redis;

public class RedisCacheService : IRedisService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly StackExchange.Redis.IDatabase _db;

    public ILogger<BusinessGetAllService> Logger { get; }

    public RedisCacheService(IOptions<RedisConfig> config, ILogger<BusinessGetAllService> logger)
    {
        _redis = ConnectionMultiplexer.Connect(config.Value.ConnectionString);
        _db = _redis.GetDatabase();
        Logger = logger;
    }

    public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiration);
        }
        catch (RedisException ex)
        {
            Console.WriteLine($"[Redis Error] The key could not be set '{key}': {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[General error] setting cache: {ex.Message}");
        }
    }



    public async Task<T?> GetCacheAsync<T>(string key)
    {
        try
        {
            var json = await _db.StringGetAsync(key);

            if (json.IsNullOrEmpty)
            {
                Logger.LogInformation("Cache Miss for key: {Key}", key);
                return default;
            }

            var deserialized = JsonSerializer.Deserialize<T>(json);

            return deserialized;
        }
        catch (RedisException ex)
        {
            Console.WriteLine($"[Redis Error] Could not get key '{key}': {ex.Message}");
            return default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[General error] getting cache: {ex.Message}");
            return default;
        }
    }



    public async Task<bool> RemoveCacheAsync(string key)
    {
        try
        {
            return await _db.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] removing cache key '{key}': {ex.Message}");
            return false;
        }
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        try
        {
            var server = _redis.GetServer(_redis.GetEndPoints()[0]);
            var keysToDelete = new List<RedisKey>();

            await foreach (var key in server.KeysAsync(pattern: $"{prefix}*"))
            {
                keysToDelete.Add(key);
            }

            if (keysToDelete.Count > 0)
            {
                await _db.KeyDeleteAsync(keysToDelete.ToArray());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] removing cache keys by prefix '{prefix}': {ex.Message}");
        }
    }
    public async Task<List<string>> GetKeysByPatternAsync(string pattern)
    {
        try
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = new List<string>();

            await foreach (var key in server.KeysAsync(pattern: $"*{pattern}*"))
            {
                keys.Add(key.ToString());
            }

            return keys;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting keys by pattern");
            return new List<string>();
        }
    }

    public async Task<string?> GetValueFromCacheAsync(string key)
    {
        var cacheKey = $"translations:en";
        var translations = await GetCacheAsync<Dictionary<string, string>>(cacheKey);
        if (translations != null && translations.ContainsKey(key))
        {
            return translations[key];  
        }
        return null;
    }
}