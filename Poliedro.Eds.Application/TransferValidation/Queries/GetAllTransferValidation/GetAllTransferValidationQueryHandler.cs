using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.TransferValidation.Dtos;
using Poliedro.Eds.Domain.TransferValidation.Entities;
using Poliedro.Eds.Domain.TransferValidation.Repositories;

namespace Poliedro.Eds.Application.TransferValidation.Queries.GetAllTransferValidation;

public class GetAllTransferValidationQueryHandler(
    ITransferValidationRepositoryGetAll transferValidationRepository,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetAllTransferValidationQueryHandler> logger,
    IMapper mapper)
    : IRequestHandler<GetAllTransferValidationQuery, IEnumerable<TransferValidationDto>>
{
    public async Task<IEnumerable<TransferValidationDto>> Handle(
        GetAllTransferValidationQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        var cacheKey = $"transferValidation:{request.PaginationParams.PageNumber}:{request.PaginationParams.PageSize}:{tenant}";

        logger.LogInformation("Buscando validaciones de transferencia en caché con clave: {CacheKey}", cacheKey);

        var cachedData = await redisService.GetCacheAsync<IEnumerable<TransferValidationEntity>>(cacheKey);
        if (cachedData is not null)
        {
            logger.LogInformation("Cache hit - Se encontraron {Count} validaciones en caché", cachedData.Count());
            return mapper.Map<IEnumerable<TransferValidationDto>>(cachedData);
        }

        logger.LogInformation("Cache miss - Consultando base de datos");
        var data = await transferValidationRepository.GetAllAsync(
        request.PaginationParams,
        cancellationToken);

        logger.LogInformation("Se obtuvieron {Count} validaciones de transferencia de la base de datos", data.Count());

        await redisService.SetCacheAsync(cacheKey, data, TimeSpan.FromMinutes(1440));
        logger.LogInformation("Datos almacenados en caché por 1440 minutos");

        return mapper.Map<IEnumerable<TransferValidationDto>>(data);
    }
}
