using Microsoft.AspNetCore.Http;

namespace Poliedro.Eds.Application.Ports.Redis;

public interface IRedisService
{
    // Métodos existentes
    Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);
    Task<T?> GetCacheAsync<T>(string key);
    Task<bool> RemoveCacheAsync(string key);
    Task RemoveByPrefixAsync(string prefix);
    Task RemoveByPrefixAsync(IEnumerable<string> prefixes);
    Task<List<string>> GetKeysByPatternAsync(string pattern);
    Task<string?> GetValueFromCacheAsync(string key);

    // Nuevos métodos para cache con tags y pub/sub
    Task SetCacheWithTagsAsync<T>(string key, T value, TimeSpan expiration, params string[] tags);
    Task InvalidateCacheByTagsAsync(params string[] tags);
    Task InvalidateCacheByTagsAsync(string tenant, params string[] tags);
    Task PublishCacheInvalidationAsync(string tenant, string[] tags);
    Task SubscribeToCacheInvalidationAsync(Func<string, string[], Task> onMessage);
    
    // Método para invalidación distribuida entre servicios
    Task InvalidateDistributedCacheAsync(string tenant, string operation, string entityType, object? entityId = null);
    
    // Helper para obtener keys con tenant scope
    string GetTenantScopedKey(string tenant, string key);
    string GetCacheTag(string tenant, string entityType);
}
