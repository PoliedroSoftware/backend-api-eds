using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Common.Helper.removekey;

public static class RedisHelper
{
    public static async Task RemoveCacheIfSuccessAsync<T>(
        Result<T, Error> result,
        IRedisService redisService,
        params string[] keys)
    {
        if (result.IsSuccess && keys.Length > 0)
            await redisService.RemoveByPrefixAsync(keys);
    }
}

