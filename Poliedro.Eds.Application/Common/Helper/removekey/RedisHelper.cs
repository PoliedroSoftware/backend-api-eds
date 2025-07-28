using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Common.Helper.removekey;

public static class RedisHelper
{
    public static async Task RemoveCacheIfSuccessAsync(
        Result<VoidResult, Error> result,
        IRedisService redisService,
        string redisKeyPrefix)
    {
        if (result.IsSuccess)
        {
            await redisService.RemoveByPrefixAsync(redisKeyPrefix);
        }
    }
}
