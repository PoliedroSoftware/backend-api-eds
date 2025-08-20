using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Phone.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Phone.DomainServices.GetAll;
using Poliedro.Eds.Domain.Phone.Entities;

namespace Poliedro.Eds.Application.Phone.Queries.GetAllPhones;

public class GetAllPhonesQueryHandler(
    IPhoneGetAllService phoneGetAllService,
    IRedisService redisService,
    ILogger<GetAllPhonesQueryHandler> logger,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GetAllPhonesQuery, IEnumerable<PhoneDto>>
{
    public async Task<IEnumerable<PhoneDto>> Handle(GetAllPhonesQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cacheKey = $"phone:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<PhoneEntity>>(cacheKey);
        if (cachedData is not null)
        {
            logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
            return mapper.Map<List<PhoneDto>>(cachedData);
        }
        var data = await phoneGetAllService.GetAllAsync(request.PaginationParams);
        var logData = new Dictionary<string, object>
        {
            ["Request"] = "phone",
            ["Response"] = JsonSerializer.Serialize(data),
            ["TraceId"] = cacheKey
        };
        logger.LogInformation("Response Phone: {@LogData}", logData);
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));
        return mapper.Map<List<PhoneDto>>(data);
    }
}
