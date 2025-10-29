using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class HoseGetAllHose(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    ) : IHoseGetAllHose
{
    public async Task<Result<IEnumerable<HoseDto>, Error>> GetAllAsync(PaginationParams paginationParams)
    {
        // Get tenant and create cache key
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cacheKey = $"hose:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";

        // Check cache first
        var cachedDtos = await redisService.GetCacheAsync<IEnumerable<HoseDto>>(cacheKey);
        if (cachedDtos is not null)
            return Result<IEnumerable<HoseDto>, Error>.Success(cachedDtos);

        // Validate if any hoses exist
        if (!await HasHosesAsync())
            return HoseErrorBuilder.HoseNotFoundException(0);

        using var context = dbContextFactory.CreateDbContext();

        // Load hoses with pagination first
        var hoses = await context.Hose
            .AsNoTracking()
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        // Get all required dispensers
        var dispenserIds = hoses.Select(h => h.IdDispensers).Distinct().ToList();
        var dispensers = await context.Dispensers
            .AsNoTracking()
            .Where(d => dispenserIds.Contains(d.Id))
            .ToListAsync();

        // Get all required EDS
        var edsIds = dispensers.Select(d => d.EdsId).Distinct().ToList();
        var edsList = await context.Eds
            .AsNoTracking()
            .Where(e => edsIds.Contains(e.IdEds))
            .ToListAsync();

        // Get all required product types
        var productTypeIds = hoses.Select(h => h.IdProductType).Distinct().ToList();
        var productTypes = await context.ProductTypes
            .AsNoTracking()
            .Where(pt => productTypeIds.Contains(pt.IdProductType))
            .ToListAsync();

        // Get all required products
        var products = await context.Product
            .AsNoTracking()
            .Where(p => productTypeIds.Contains(p.IdProductType))
            .ToListAsync();

        // Map to DTOs
        var hoseDtos = hoses.Select(hose => 
        {
            var dispenser = dispensers.FirstOrDefault(d => d.Id == hose.IdDispensers);
            var eds = dispenser != null ? edsList.FirstOrDefault(e => e.IdEds == dispenser.EdsId) : null;
            var productType = productTypes.FirstOrDefault(pt => pt.IdProductType == hose.IdProductType);
            var product = products.FirstOrDefault(p => p.IdProductType == hose.IdProductType);

            // Create clean DTOs
            var cleanEds = eds != null ? new CleanEdsDto(
                eds.IdEds,
                eds.Name,
                eds.Nit,
                eds.Address,
                eds.Sicom,
                eds.IdBusiness
            ) : null;

            var cleanDispenser = dispenser != null ? new CleanDispensersDto(
                dispenser.Id,
                dispenser.Code,
                dispenser.Number,
                dispenser.DispenserTypeId,
                dispenser.EdsId,
                dispenser.IdIsland,
                dispenser.HoseNumber
            ) : null;

            var cleanProductType = productType != null ? new CleanProductTypeDto(
                productType.IdProductType,
                productType.Description
            ) : null;

            var cleanProduct = product != null ? new CleanProductDto(
                product.IdProduct,
                product.Name,
                product.IdProductType,
                product.PurchasePrice,
                product.SellPrice,
                product.Stock,
                product.Date
            ) : null;

            return new HoseDto(
                hose.IdHose,
                hose.IdDispensers,
                hose.Number,
                hose.AccumulatedAmount,
                hose.AccumulatedGallons,
                hose.IdProductType,
                hose.IdCompartiment,
                cleanDispenser,
                cleanProductType,
                cleanEds,
                cleanProduct,
                string.Empty,
                0
            );
        }).ToList();

        // Cache the results
        await redisService.SetCacheAsync(cacheKey, hoseDtos, TimeSpan.FromMinutes(1440));

        return Result<IEnumerable<HoseDto>, Error>.Success(hoseDtos);
    }

    private async Task<bool> HasHosesAsync()
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Hose
            .AsNoTracking()
            .AnyAsync();
    }
}
