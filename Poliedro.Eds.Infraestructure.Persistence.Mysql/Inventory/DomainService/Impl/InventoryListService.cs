using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Inventory.DomainService;
using Poliedro.Eds.Domain.Inventory.Dto.View;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using StackExchange.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Inventory.Repositories;

public class InventoryListService(IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    ITenantDbContextFactory dbContextFactory, ILogger<InventoryListService> logger) : IInventoryListDomainService
{
    public async Task<IEnumerable<InventoryListResponseDto>> GetAllAsync(PaginationParams paginationParams)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString() ?? "unknown";
        string cachekey = $"inventoryListService:{tenant}:{paginationParams.PageNumber}:{paginationParams.PageSize}";

        var cachedData = await redisService.GetCacheAsync<IEnumerable<InventoryListResponseDto>>(cachekey);
        if (cachedData != null) return cachedData;

        try
        {
            var inventories = await GetInventoriesFromViewAsync();

            var pagedInventories = inventories
                .Skip(Math.Max(0, (paginationParams.PageNumber <= 0 ? 0 : (paginationParams.PageNumber - 1)) * paginationParams.PageSize))
                .Take(paginationParams.PageSize)
                .ToList();

            if (pagedInventories.Count > 0)
                await redisService.SetCacheAsync(cachekey, pagedInventories, TimeSpan.FromHours(24));

            return pagedInventories;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting inventory list for tenant {Tenant} with params {@Pagination}", tenant, paginationParams);
            
            return [];
        }
    }


    private T GetValueOrDefault<T>(DbDataReader reader, string col, T defaultValue = default!)
    {
        var idx = reader.GetOrdinal(col);
        if (reader.IsDBNull(idx)) return defaultValue;

        object val = reader.GetValue(idx);
        
        try
        {
            if (typeof(T) == typeof(double))
                return (T)(object)Convert.ToDouble(val, System.Globalization.CultureInfo.InvariantCulture);
            if (typeof(T) == typeof(int))
                return (T)(object)Convert.ToInt32(val, System.Globalization.CultureInfo.InvariantCulture);
            if (typeof(T) == typeof(string))
                return (T)((T)(object)Convert.ToString(val) ?? (object)defaultValue!);
            if (typeof(T) == typeof(long))
                return (T)(object)Convert.ToInt64(val, System.Globalization.CultureInfo.InvariantCulture);
            if (typeof(T) == typeof(float))
                return (T)(object)Convert.ToSingle(val, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (InvalidCastException ex)
        {
            logger.LogError(ex, "Failed to convert column {Column} to type {Type} in GetValueOrDefault.", col, typeof(T));
        }
        catch (FormatException ex)
        {
            logger.LogError(ex, "Failed to convert column {Column} to type {Type} in GetValueOrDefault.", col, typeof(T));
        }
        catch (OverflowException ex)
        {
            logger.LogError(ex, "Failed to convert column {Column} to type {Type} in GetValueOrDefault.", col, typeof(T));
        }

        return (T)val; 
    }

    private async Task<IEnumerable<InventoryListResponseDto>> GetInventoriesFromViewAsync()
    {
        using var context = dbContextFactory.CreateDbContext();
        await using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        const string query = "SELECT * FROM v_inventory";
        await using var command = connection.CreateCommand();
        command.CommandText = query;

        var rows = new List<(int IdBusiness, string Business, int IdEds, string Eds, int IdTank, string Tank, double TankCapacity, int IdCompartment, int Compartment, int IdProduct, string Product, double Stock)>();

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            rows.Add((
                IdBusiness: reader.GetInt32(reader.GetOrdinal("business_id")),
                Business: reader.GetString(reader.GetOrdinal("business")),
                IdEds: reader.GetInt32(reader.GetOrdinal("eds_id")),
                Eds: reader.GetString(reader.GetOrdinal("eds")),
                IdTank: reader.GetInt32(reader.GetOrdinal("tank_id")),
                Tank: reader.GetString(reader.GetOrdinal("tank")),
                TankCapacity: reader.GetDouble(reader.GetOrdinal("tank_capacity")),
                IdCompartment: reader.GetInt32(reader.GetOrdinal("compartment_id")),
                Compartment: reader.GetInt32(reader.GetOrdinal("compartment")),
                IdProduct: reader.GetInt32(reader.GetOrdinal("product_id")),
                Product: reader.GetString(reader.GetOrdinal("product")),
                Stock: reader.IsDBNull(reader.GetOrdinal("stock"))
                                ? 0
                                : reader.GetDouble(reader.GetOrdinal("stock"))
            ));
        }




        if (rows.Count == 0)
        {
            logger.LogWarning("v_inventory returned 0 rows");
          
            return [];
        }

        var inventoryList =
            rows.GroupBy(r => new { r.IdBusiness, r.Business })
                .Select(b => new InventoryListResponseDto
                {                                  
                    Businesses = new[]
                    {
                    new BusinessDto
                    {
                        IdBusiness = b.Key.IdBusiness,
                        Business = b.Key.Business,
                        Eds = b.GroupBy(e => new { e.IdEds, e.Eds })
                            .Select(e => new EdsDto
                            {
                                IdEds = e.Key.IdEds,
                                Eds = e.Key.Eds,
                                Tanks = e.GroupBy(t => new { t.IdTank, t.Tank, t.TankCapacity })
                                    .Select(t => new TankDto
                                    {
                                        IdTank = t.Key.IdTank,
                                        Tank = t.Key.Tank,
                                        TankCapacity = t.Key.TankCapacity,
                                        Compartments = t.GroupBy(c => new { c.IdCompartment, c.Compartment, c.IdProduct })
                                            .Select(cg => new CompartmentDto
                                            {
                                                IdCompartment = cg.Key.IdCompartment,
                                                Compartment = cg.Key.Compartment,
                                                IdProduct = cg.Key.IdProduct,
                                                Product = cg.First().Product,
                                                Stock = cg.First().Stock
                                            }).ToList()
                                    }).ToList()
                            }).ToList()
                    }
                    }
                }).ToList();

        return inventoryList;
    }
}
