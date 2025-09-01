using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Poliedro.Eds.Application.Hose.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
=======
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
>>>>>>> New-service-StrongBox
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
<<<<<<< HEAD
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
                cleanProduct
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
=======
    public async Task<IEnumerable<HoseDto>> GetAllAsync(PaginationParams paginationParams)
    {

        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cacheKey = $"hose:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";

        var cachedDtos = await redisService.GetCacheAsync<IEnumerable<HoseDto>>(cacheKey);
        if (cachedDtos is not null)
            return cachedDtos;

        using var context = dbContextFactory.CreateDbContext();

        var validIds = await context.Hose.Select(h => h.IdHose).ToListAsync();

        var query = from hose in context.Hose
                    where validIds.Contains(hose.IdHose)
                    join dispenser in context.Dispensers on hose.IdDispensers equals dispenser.Id
                    join productType in context.ProductTypes on hose.IdProductType equals productType.IdProductType
                    join eds in context.Eds on dispenser.EdsId equals eds.IdEds
                    join product in context.Product on hose.IdProductType equals product.IdProductType
                    group new { hose, dispenser, productType, eds, product } by hose.IdHose into g
                    select new HoseDto(
                        g.First().hose.IdHose,
                        g.First().hose.Number,
                        g.First().hose.IdDispensers,
                        g.First().hose.AccumulatedGallons,
                        g.First().hose.AccumulatedAmount,
                        g.First().hose.IdProductType,
                        g.First().product.Price,
                        g.First().dispenser,
                        g.First().productType,
                        g.First().eds
                    );

        var hoseDtos = (await query
            .ToListAsync())
            .GroupBy(h => h.IdHose)
            .Select(g => g.First())
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToList();


        await redisService.SetCacheAsync(cacheKey, hoseDtos, TimeSpan.FromMinutes(1440));

        return hoseDtos;
    }

>>>>>>> New-service-StrongBox
}
