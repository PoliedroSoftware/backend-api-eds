using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Application.Common.Services.Cache;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItem, TimeSpan expiration, string[] tags, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, TimeSpan expiration, string[] tags, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByTagsAsync(string[] tags, CancellationToken cancellationToken = default);
    string GetTenantScopedKey(string key);
}

public class CacheService : ICacheService
{
    private readonly IRedisService _redisService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CacheService(IRedisService redisService, IHttpContextAccessor httpContextAccessor)
    {
        _redisService = redisService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var scopedKey = GetTenantScopedKey(key);
        return await _redisService.GetCacheAsync<T>(scopedKey);
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItem, TimeSpan expiration, string[] tags, CancellationToken cancellationToken = default)
    {
        var scopedKey = GetTenantScopedKey(key);
        var cachedValue = await _redisService.GetCacheAsync<T>(scopedKey);
        
        if (cachedValue != null)
        {
            return cachedValue;
        }

        var value = await getItem();
        await SetAsync(key, value, expiration, tags, cancellationToken);
        
        return value;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, string[] tags, CancellationToken cancellationToken = default)
    {
        var scopedKey = GetTenantScopedKey(key);
        var scopedTags = GetTenantScopedTags(tags);
        
        await _redisService.SetCacheWithTagsAsync(scopedKey, value, expiration, scopedTags);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        var scopedKey = GetTenantScopedKey(key);
        await _redisService.RemoveCacheAsync(scopedKey);
    }

    public async Task RemoveByTagsAsync(string[] tags, CancellationToken cancellationToken = default)
    {
        var tenant = GetCurrentTenant();
        if (string.IsNullOrEmpty(tenant)) return;
        
        await _redisService.InvalidateCacheByTagsAsync(tenant, tags);
    }

    public string GetTenantScopedKey(string key)
    {
        var tenant = GetCurrentTenant();
        return string.IsNullOrEmpty(tenant) ? key : _redisService.GetTenantScopedKey(tenant, key);
    }

    private string[] GetTenantScopedTags(string[] tags)
    {
        var tenant = GetCurrentTenant();
        if (string.IsNullOrEmpty(tenant)) return tags;
        
        return tags.Select(tag => _redisService.GetCacheTag(tenant, tag)).ToArray();
    }

    private string? GetCurrentTenant()
    {
        return _httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
    }
}
