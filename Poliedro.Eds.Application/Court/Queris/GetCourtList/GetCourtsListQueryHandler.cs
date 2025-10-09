using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Poliedro.Eds.Application.Court.Dtos.View;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.Court.DomainService;

namespace Poliedro.Eds.Application.Court.Queris.GetCourtList;

public class GetCourtsListQueryHandler(
    ICourtListDomainService courtListDomainService,
    IHttpContextAccessor httpContextAccessor,
    IRedisService redisService,
    IMapper mapper) : IRequestHandler<GetCourtsListQuery, IEnumerable<CourtListResponseDto>>
{
    public async Task<IEnumerable<CourtListResponseDto>> Handle(GetCourtsListQuery request, CancellationToken cancellationToken)
    {
        var username = httpContextAccessor.HttpContext?.Items["identifiername"]?.ToString();
        var roles = httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        bool isAdmin = roles.Contains("Admin");

        string userKeyPart = roles.Contains("Admin") ? "admin" : $"user:{username}";
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();

        var paginationParams = request.PaginationParams;

        string cacheKey = $"courtListService:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}:{userKeyPart}";

        var cachedData = await redisService.GetCacheAsync<IEnumerable<CourtListResponseDto>>(cacheKey);
        if (cachedData != null)
            return cachedData;

        var entities = await courtListDomainService.GetAllAsync(paginationParams, username!, isAdmin);
        var dtos = mapper.Map<IEnumerable<CourtListResponseDto>>(entities);

        await redisService.SetCacheAsync(cacheKey, dtos, TimeSpan.FromMinutes(5));
        return dtos;
    }
}

