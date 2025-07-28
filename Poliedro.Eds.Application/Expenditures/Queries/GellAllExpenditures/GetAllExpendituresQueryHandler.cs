using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Expenditures.DomainExpenditures;
using Poliedro.Eds.Domain.Expenditures.Entities;

namespace Poliedro.Eds.Application.Expenditures.Queries.GellAllExpenditures;

public class GetAllExpendituresQueryHandler
(
    IExpendituresGetAllExpenditures ExpendituresGetAllService,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GellAllExpendituresQuery, IEnumerable<ExpendituresDto>>
{
    public async Task<IEnumerable<ExpendituresDto>> Handle(GellAllExpendituresQuery request, CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext.Items["tenant"]?.ToString();
        var cacheKey = $"expenditures:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<ExpendituresEntity>>(cacheKey);
        if (cachedData is not null)
        {
            return mapper.Map<IEnumerable<ExpendituresDto>>(cachedData);
        }

        var data = await ExpendituresGetAllService.GetAllAsync(request.PaginationParams);

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));

        return mapper.Map<IEnumerable<ExpendituresDto>>(data);
    }
}


