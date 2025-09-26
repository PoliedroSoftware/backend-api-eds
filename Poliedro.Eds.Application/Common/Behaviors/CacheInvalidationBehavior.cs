using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Common.Events.Cache;
using System.Reflection;

namespace Poliedro.Eds.Application.Common.Behaviors;


public class CacheInvalidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CacheInvalidationBehavior<TRequest, TResponse>> _logger;

    public CacheInvalidationBehavior(
        IDomainEventDispatcher domainEventDispatcher,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CacheInvalidationBehavior<TRequest, TResponse>> logger)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (IsWriteCommand(request) && IsSuccessfulResponse(response))
        {
            var tenant = GetTenantFromContext();
            if (string.IsNullOrEmpty(tenant))
            {
                _logger.LogWarning("⚠️ No se pudo obtener el tenant del contexto para invalidación de caché");
                return response;
            }

            var cacheMetadata = GetCacheInvalidationMetadata(request);
            if (cacheMetadata != null)
            {
                _logger.LogInformation("🗑️ Disparando invalidación de caché automática para comando '{CommandType}'",
                    typeof(TRequest).Name);

                var domainEvent = new CacheInvalidationDomainEvent(
                    tenant,
                    cacheMetadata.Operation,
                    cacheMetadata.EntityType,
                    cacheMetadata.EntityId,
                    cacheMetadata.AdditionalTags);

                await _domainEventDispatcher.DispatchAsync(domainEvent, cancellationToken);
            }
        }

        return response;
    }

    private static bool IsWriteCommand(TRequest request)
    {
        var requestType = typeof(TRequest);
        var typeName = requestType.Name.ToLowerInvariant();
        
        return typeName.Contains("create") || 
               typeName.Contains("update") || 
               typeName.Contains("delete") ||
               typeName.Contains("command");
    }

    private static bool IsSuccessfulResponse(TResponse response)
    {
        if (response == null) return false;
        
        var responseType = typeof(TResponse);
        
        if (responseType.IsGenericType && responseType.Name.StartsWith("Result"))
        {
            var isSuccessProperty = responseType.GetProperty("IsSuccess");
            if (isSuccessProperty != null)
            {
                return (bool)(isSuccessProperty.GetValue(response) ?? false);
            }
        }

        return true;
    }

    private string? GetTenantFromContext()
    {
        return _httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
    }

    private static CacheInvalidationMetadata? GetCacheInvalidationMetadata(TRequest request)
    {
        var requestType = typeof(TRequest);
        var typeName = requestType.Name.ToLowerInvariant();

        string operation = ExtractOperation(typeName);
        string entityType = ExtractEntityType(typeName);
        
        if (string.IsNullOrEmpty(operation) || string.IsNullOrEmpty(entityType))
        {
            return null;
        }

        object? entityId = ExtractEntityId(request);

        return new CacheInvalidationMetadata
        {
            Operation = operation,
            EntityType = entityType,
            EntityId = entityId,
            AdditionalTags = Array.Empty<string>()
        };
    }

    private static string ExtractOperation(string commandName)
    {
        if (commandName.Contains("create")) return "create";
        if (commandName.Contains("update")) return "update";
        if (commandName.Contains("delete")) return "delete";
        
        return "modify"; 
    }

    private static string ExtractEntityType(string commandName)
    {
        var cleaned = commandName
            .Replace("create", "")
            .Replace("update", "")
            .Replace("delete", "")
            .Replace("command", "")
            .Replace("request", "")
            .Trim();

        return cleaned;
    }

    private static object? ExtractEntityId(TRequest request)
    {
        var requestType = typeof(TRequest);
        
        var idProperties = requestType.GetProperties()
            .Where(p => p.Name.ToLowerInvariant().Contains("id"))
            .ToArray();

        if (idProperties.Any())
        {
            var idProperty = idProperties.FirstOrDefault(p => 
                p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
                p.Name.Equals("IdEntity", StringComparison.OrdinalIgnoreCase)) 
                ?? idProperties.First();

            return idProperty.GetValue(request);
        }

        return null;
    }

    private class CacheInvalidationMetadata
    {
        public string Operation { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public object? EntityId { get; set; }
        public string[] AdditionalTags { get; set; } = Array.Empty<string>();
    }
}
