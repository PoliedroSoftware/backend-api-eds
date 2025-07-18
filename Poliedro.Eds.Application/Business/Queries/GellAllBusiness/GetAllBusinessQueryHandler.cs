using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Business.DomainBusiness;
using Poliedro.Eds.Domain.Business.Entities;
using System.Text.Json;

namespace Poliedro.Eds.Application.Business.Queries.GellAllBusiness;
public class GetAllBusinessQueryHandler
(
    IBusinessGetAllService businessGetAllService,
    IRedisService redisService,
    ILogger<GetAllBusinessQueryHandler> logger,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllBusinessQuery, IEnumerable<BusinessDto>>
{
    public async Task<IEnumerable<BusinessDto>> Handle(GellAllBusinessQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cacheKey = $"business:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<BusinessEntity>>(cacheKey);
        if (cachedData is not null)
        {
            logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
            return mapper.Map<List<BusinessDto>>(cachedData);
        }
        var data = await businessGetAllService.GetAllAsync(request.PaginationParams);
        var logData = new Dictionary<string, object>
        {
            ["Request"] = "business",
            ["Response"] = JsonSerializer.Serialize(data),
            ["TraceId"] = cacheKey
        };
        logger.LogInformation("Response Business: {@LogData}", logData);
        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));
        return mapper.Map<List<BusinessDto>>(data);
    }
}