using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Compartiment.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Application.Compartiment.Queries.GellAllCompartiment;
public class GetAllCompartimentQueryHandler
(
    ICompartimentGetAllService CompartimentGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllCompartimentQuery, IEnumerable<CompartimentDto>>
{
    public async Task<IEnumerable<CompartimentDto>> Handle(GellAllCompartimentQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"compartiment:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CompartimentEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<CompartimentDto>>(cachedData);
        }

        var data = await CompartimentGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<CompartimentDto>>(data);
    }
}


