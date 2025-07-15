using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Inventory.DomainService;
using Poliedro.Eds.Domain.Inventory.Dto.View;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Inventory.Repositories;

public class InventoryListService(
    IConfiguration config,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    ITenantDbContextFactory dbContextFactory
    ) : IInventoryListDomainService
{
    //private readonly string _connectionString = config["ConnectionStrings:MysqlConnection"];

    public async Task<IEnumerable<InventoryListResponseDto>> GetAllAsync(PaginationParams paginationParams)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cachekey = $"inventoryListService:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<InventoryListResponseDto>>(cachekey);

        if (cachedData != null) return cachedData;
        try
        {
            var inventories = await GetInventoriesFromViewAsync();

            var pagedInventories = inventories
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            await redisService.SetCacheAsync(cachekey, pagedInventories, TimeSpan.FromMinutes(1440));
            return pagedInventories;
        }
        catch (Exception)
        {
            return [];
        }
    }

    private async Task<IEnumerable<InventoryListResponseDto>> GetInventoriesFromViewAsync()
    {
        using var context = dbContextFactory.CreateDbContext();
        var rows = new List<dynamic>();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();
        //using var connection = new MySqlConnection(_connectionString);
        //await connection.OpenAsync();

        string query = "SELECT * FROM v_inventory";
        using var command = connection.CreateCommand();
        command.CommandText = query;
        //using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            rows.Add(new
            {
                IdBusiness = reader.GetInt32(reader.GetOrdinal("id_business")),
                Business = reader.GetString(reader.GetOrdinal("business")),
                IdEds = reader.GetInt32(reader.GetOrdinal("id_eds")),
                Eds = reader.GetString(reader.GetOrdinal("eds")),
                IdTank = reader.GetInt32(reader.GetOrdinal("id_tank")),
                Tank = reader.GetString(reader.GetOrdinal("tank")),
                TankCapacity = reader.GetDouble(reader.GetOrdinal("tank_capacity")),
                IdCompartment = reader.GetInt32(reader.GetOrdinal("id_compartment")),
                Compartment = reader.GetInt32(reader.GetOrdinal("compartment")),
                IdProduct = reader.GetInt32(reader.GetOrdinal("id_product")),
                Product = reader.GetString(reader.GetOrdinal("product")),
                Stock = reader.GetDouble(reader.GetOrdinal("stock"))
            });
        }

        var inventoryList = rows
            .GroupBy(r => new { r.IdBusiness, r.Business })
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
                                            .Select(c => new CompartmentDto
                                            {
                                                IdCompartment = c.Key.IdCompartment,
                                                Compartment = c.Key.Compartment,
                                                IdProduct = c.Key.IdProduct,
                                                Product = c.First().Product,
                                                Stock = c.First().Stock
                                            }).ToList()
                                    }).ToList()
                            }).ToList()
                    }
                }
            }).ToList();

        return inventoryList;
    }
}
