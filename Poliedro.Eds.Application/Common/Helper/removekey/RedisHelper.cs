using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Events.Cache;
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


    public static async Task InvalidateDistributedCacheAsync<T>(
        Result<T, Error> result,
        IRedisService redisService,
        IDomainEventDispatcher eventDispatcher,
        IHttpContextAccessor httpContextAccessor,
        string entityType,
        string operation = "modify",
        object? entityId = null)
    {
        if (!result.IsSuccess) return;

        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        if (string.IsNullOrEmpty(tenant)) return;

        await redisService.InvalidateDistributedCacheAsync(tenant, operation, entityType, entityId);
        
        var domainEvent = new EntityModifiedDomainEvent(
            tenant, 
            entityType, 
            entityId ?? Guid.NewGuid(), 
            MapOperationToEnum(operation),
            result.Value);

        await eventDispatcher.DispatchAsync(domainEvent);
    }

    public static async Task InvalidateCacheByTagsAsync<T>(
        Result<T, Error> result,
        IRedisService redisService,
        IHttpContextAccessor httpContextAccessor,
        params string[] entityTypes)
    {
        if (!result.IsSuccess || entityTypes.Length == 0) return;

        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        if (string.IsNullOrEmpty(tenant)) return;

        await redisService.InvalidateCacheByTagsAsync(tenant, entityTypes);
    }

    private static EntityOperation MapOperationToEnum(string operation)
    {
        return operation.ToLowerInvariant() switch
        {
            "create" => EntityOperation.Created,
            "update" => EntityOperation.Updated,
            "delete" => EntityOperation.Deleted,
            _ => EntityOperation.Updated
        };
    }
}

