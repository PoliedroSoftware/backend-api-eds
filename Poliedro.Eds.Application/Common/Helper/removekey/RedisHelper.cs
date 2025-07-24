
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Helpers;

public static class RedisHelper
{
    public static async Task RemoveBusinessCacheIfSuccessAsync(
        Result<VoidResult, Error> result,
        IRedisService redisService)
    {
        if (result.IsSuccess)
        {
            await redisService.RemoveByPrefixAsync(KeyRedisConstants.BUSINESS);
        }
    }
}
