using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using Poliedro.Eds.Application.Court.Dtos.View;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Poliedro.Eds.Application.Court.Queris.GetCourtList;

public class GetCourtsListQueryHandler(
    ICourtListDomainService courtListDomainService,
    IHttpContextAccessor httpContextAccessor,
    IRedisService redisService,
    IMapper mapper,
    IS3UrlGenerator s3UrlGenerator) : IRequestHandler<GetCourtsListQuery, IEnumerable<CourtListResponseDto>>
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

        // Generar URLs pre-firmadas para los documentos de cada corte
        foreach (var courtDto in dtos)
        {
            if (courtDto.Documents != null && courtDto.Documents.Any())
            {
                foreach (var document in courtDto.Documents)
                {
                    if (!string.IsNullOrWhiteSpace(document.DocumentName))
                    {
                        // Generar URL pre-firmada (expira en 24 horas)
                        document.DocumentName = await s3UrlGenerator.GeneratePresignedUrlAsync(
                            document.DocumentName,
                            expirationMinutes: 1440);
                    }
                }
            }
        }

        await redisService.SetCacheAsync(cacheKey, dtos, TimeSpan.FromMinutes(5));
        return dtos;
    }
}

