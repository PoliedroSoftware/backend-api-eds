using Microsoft.AspNetCore.Http;

namespace Poliedro.Eds.Application.Ports.Redis;

public interface IRedisService
{
    Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);
    Task<T?> GetCacheAsync<T>(string key);
    Task<bool> RemoveCacheAsync(string key);
    Task RemoveByPrefixAsync(string prefix);
    Task RemoveByPrefixAsync(IEnumerable<string> prefixes);
    Task<List<string>> GetKeysByPatternAsync(string pattern);
    Task<string?> GetValueFromCacheAsync(string key);
}
