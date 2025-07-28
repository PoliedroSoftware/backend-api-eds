using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.TypeOfCollection.Dtos;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Application.TypeOfCollection.Queries.GellAllTypeOfCollection;

public class GetAllTypeOfCollectionQueryHandler
(
    ITypeOfCollectionGetAllTypeOfCollection TypeOfCollectionGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllTypeOfCollectionQuery, IEnumerable<TypeOfCollectionDto>>
{
    public async Task<IEnumerable<TypeOfCollectionDto>> Handle(GellAllTypeOfCollectionQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"typeOfCollection:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";

        var cachedData = await redisService.GetCacheAsync<IEnumerable<TypeOfCollectionEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<TypeOfCollectionDto>>(cachedData);
        }

        var data = await TypeOfCollectionGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<TypeOfCollectionDto>>(data);
    }
}