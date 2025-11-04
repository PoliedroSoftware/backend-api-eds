using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;
using StackExchange.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Redis;

public class RedisCacheService : IRedisService
{
    private readonly ConnectionMultiplexer? _redis;
    private readonly IDatabase? _db;
    private readonly ISubscriber? _subscriber;
    private readonly IServer? _server;
    private readonly ILogger<BusinessGetAllService> _logger;
    private readonly bool _isConnected;

    private const string CACHE_TAGS_PREFIX = "cache:tags:";
    private const string CACHE_INVALIDATION_CHANNEL = "cache:invalidation";

    public RedisCacheService(
        IOptions<RedisConfig> config,
        ILogger<BusinessGetAllService> logger
       )
    {
        _logger = logger;
        _isConnected = false;

        try
        {
            _redis = ConnectionMultiplexer.Connect(config.Value.ConnectionString);
            _db = _redis.GetDatabase();
            _subscriber = _redis.GetSubscriber();
            _server = _redis.GetServer(_redis.GetEndPoints()[0]);
            _isConnected = true;
            _logger.LogInformation("Successfully connected to Redis at {ConnectionString}", config.Value.ConnectionString);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to connect to Redis. The application will continue without Redis caching functionality.");
        }
    }

    public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
    {
        if (!_isConnected || _db == null)
        {
            _logger.LogDebug("Redis not available. Skipping cache set for key '{Key}'", key);
            return;
        }

        try
        {
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiration);
        }
        catch (RedisException ex)
        {
            _logger.LogError(ex, "[Redis Error] The key could not be set '{Key}'", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[General error] setting cache");
        }
    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        if (!_isConnected || _db == null)
        {
            _logger.LogDebug("Redis not available. Cache miss for key '{Key}'", key);
            return default;
        }

        try
        {
            var json = await _db.StringGetAsync(key);

            if (json.IsNullOrEmpty)
            {
                _logger.LogInformation("Cache Miss for key: {Key}", key);
                return default;
            }

            var deserialized = JsonSerializer.Deserialize<T>(json);
            return deserialized;
        }
        catch (RedisException ex)
        {
            _logger.LogError(ex, "[Redis Error] Could not get key '{Key}'", key);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[General error] getting cache");
            return default;
        }
    }

    public async Task<bool> RemoveCacheAsync(string key)
    {
        if (!_isConnected || _db == null)
        {
            _logger.LogDebug("Redis not available. Skipping cache remove for key '{Key}'", key);
            return false;
        }

        try
        {
            return await _db.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Error] removing cache key '{Key}'", key);
            return false;
        }
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        if (!_isConnected || _db == null || _server == null)
        {
            _logger.LogDebug("Redis not available. Skipping cache remove by prefix '{Prefix}'", prefix);
            return;
        }

        try
        {
            var keysToDelete = new List<RedisKey>();

            await foreach (var key in _server.KeysAsync(pattern: $"{prefix}*"))
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
            _logger.LogError(ex, "[Error] removing cache keys by prefix '{Prefix}'", prefix);
        }
    }

    public async Task RemoveByPrefixAsync(IEnumerable<string> prefixes)
    {
        if (!_isConnected)
        {
            return;
        }

        var tasks = prefixes.Select(prefix => RemoveByPrefixAsync(prefix));
        await Task.WhenAll(tasks);
    }

    public async Task<List<string>> GetKeysByPatternAsync(string pattern)
    {
        if (!_isConnected || _server == null)
        {
            _logger.LogDebug("Redis not available. Returning empty list for pattern '{Pattern}'", pattern);
            return new List<string>();
        }

        try
        {
            var keys = new List<string>();

            await foreach (var key in _server.KeysAsync(pattern: $"*{pattern}*"))
            {
                keys.Add(key.ToString());
            }

            return keys;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting keys by pattern");
            return new List<string>();
        }
    }

    public async Task<string?> GetValueFromCacheAsync(string key)
    {
        if (!_isConnected)
        {
            return null;
        }

        var cacheKey = $"translations:en";
        var translations = await GetCacheAsync<Dictionary<string, string>>(cacheKey);
        if (translations != null && translations.ContainsKey(key))
        {
            return translations[key];
        }
        return null;
    }

    public async Task SetCacheWithTagsAsync<T>(string key, T value, TimeSpan expiration, params string[] tags)
    {
        if (!_isConnected || _db == null)
        {
            _logger.LogDebug("Redis not available. Skipping cache set with tags for key '{Key}'", key);
            return;
        }

        try
        {
            // Guardar el valor principal
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiration);

            // Asociar el key con cada tag
            foreach (var tag in tags)
            {
                var tagKey = $"{CACHE_TAGS_PREFIX}{tag}";
                await _db.SetAddAsync(tagKey, key);
                await _db.KeyExpireAsync(tagKey, expiration.Add(TimeSpan.FromMinutes(5))); // Expirar tags un poco después
            }

            _logger.LogDebug("Cache set with key '{Key}' and tags: {Tags}", key, string.Join(", ", tags));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Error] setting cache with tags for key '{Key}'", key);
        }
    }

    public async Task InvalidateCacheByTagsAsync(params string[] tags)
    {
        if (!_isConnected || _db == null)
        {
            _logger.LogDebug("Redis not available. Skipping cache invalidation by tags");
            return;
        }

        try
        {
            var keysToDelete = new HashSet<RedisKey>();

            foreach (var tag in tags)
            {
                var tagKey = $"{CACHE_TAGS_PREFIX}{tag}";
                var taggedKeys = await _db.SetMembersAsync(tagKey);

                foreach (var key in taggedKeys)
                {
                    keysToDelete.Add(key.ToString()); // Convertir RedisValue a string y luego a RedisKey implícitamente
                }

                // Eliminar el tag también
                keysToDelete.Add(tagKey);
            }

            if (keysToDelete.Count > 0)
            {
                var deletedCount = await _db.KeyDeleteAsync(keysToDelete.ToArray());
                _logger.LogInformation("Cache invalidated by tags {Tags}: {DeletedCount} keys deleted",
                 string.Join(", ", tags), deletedCount);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Error] invalidating cache by tags: {Tags}", string.Join(", ", tags));
        }
    }

    public async Task InvalidateCacheByTagsAsync(string tenant, params string[] tags)
    {
        if (!_isConnected)
        {
            return;
        }

        try
        {
            var tenantScopedTags = tags.Select(tag => GetCacheTag(tenant, tag)).ToArray();
            await InvalidateCacheByTagsAsync(tenantScopedTags);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Error] invalidating cache by tenant '{Tenant}' and tags: {Tags}",
         tenant, string.Join(", ", tags));
        }
    }

    public async Task PublishCacheInvalidationAsync(string tenant, string[] tags)
    {
        if (!_isConnected || _subscriber == null)
        {
            _logger.LogDebug("Redis not available. Skipping cache invalidation publish");
            return;
        }

        try
        {
            var message = JsonSerializer.Serialize(new
            {
                Tenant = tenant,
                Tags = tags,
                Timestamp = DateTimeOffset.UtcNow
            });

            await _subscriber.PublishAsync(CACHE_INVALIDATION_CHANNEL, message);
            _logger.LogInformation("Published cache invalidation for tenant '{Tenant}' with tags: {Tags}",
     tenant, string.Join(", ", tags));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Error] publishing cache invalidation message");
        }
    }

    public async Task SubscribeToCacheInvalidationAsync(Func<string, string[], Task> onMessage)
    {
        if (!_isConnected || _subscriber == null)
        {
            _logger.LogDebug("Redis not available. Skipping cache invalidation subscription");
            return;
        }

        try
        {
            await _subscriber.SubscribeAsync(CACHE_INVALIDATION_CHANNEL, async (channel, message) =>
            {
                try
                {
                    // Convertir explícitamente a string para evitar ambigüedad
                    var messageString = message.ToString();
                    using var document = JsonDocument.Parse(messageString);
                    var root = document.RootElement;

                    var tenant = root.GetProperty("Tenant").GetString() ?? string.Empty;
                    var tags = root.GetProperty("Tags")
                         .EnumerateArray()
                   .Select(x => x.GetString())
                .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray()!;

                    await onMessage(tenant, tags);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[Error] processing cache invalidation message: {Message}", message);
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Error] subscribing to cache invalidation");
        }
    }

    public async Task InvalidateDistributedCacheAsync(string tenant, string operation, string entityType, object? entityId = null)
    {
        if (!_isConnected)
        {
            return;
        }

        try
        {
            var tags = new List<string>
            {
        GetCacheTag(tenant, entityType),
  GetCacheTag(tenant, "all") // Tag global para invalidar todo
    };

            // Agregar tags específicos basados en la operación
            switch (operation.ToLower())
            {
                case "create":
                case "update":
                case "delete":
                    // Invalidar listas y entidades relacionadas
                    tags.Add(GetCacheTag(tenant, $"{entityType}:list"));
                    if (entityId != null)
                    {
                        tags.Add(GetCacheTag(tenant, $"{entityType}:{entityId}"));
                    }
                    break;
            }

            // Invalidar localmente primero
            await InvalidateCacheByTagsAsync(tags.ToArray());

            // Luego notificar a otros servicios
            await PublishCacheInvalidationAsync(tenant, tags.ToArray());

            _logger.LogInformation("Distributed cache invalidation completed for tenant '{Tenant}', operation '{Operation}', entity '{EntityType}'",
     tenant, operation, entityType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Error] invalidating distributed cache");
        }
    }

    public string GetTenantScopedKey(string tenant, string key)
    {
        return $"{tenant}:{key}";
    }

    public string GetCacheTag(string tenant, string entityType)
    {
        return $"{tenant}:{entityType}";
    }

}
