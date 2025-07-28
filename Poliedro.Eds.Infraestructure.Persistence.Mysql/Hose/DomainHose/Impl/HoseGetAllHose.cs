using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Domain.ProductType.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using System.Data;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;



public class HoseGetAllHose(
ITenantDbContextFactory dbContextFactory,
IRedisService redisService,
IHttpContextAccessor httpContextAccessor
) : IHoseGetAllHose
{   
    public async Task<IEnumerable<HoseViewDto>> GetAllAsync(PaginationParams paginationParams)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cacheKey = $"hoseListService:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";

        var cachedData = await redisService.GetCacheAsync<IEnumerable<HoseViewDto>>(cacheKey);
        if (cachedData != null) return cachedData;

        try
        {
            var hoses = await GetHosesFromViewAsync();

            var pagedHoses = hoses
                .OrderByDescending(h => h.IdHose)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            await redisService.SetCacheAsync(cacheKey, pagedHoses, TimeSpan.FromMinutes(1440));
            return pagedHoses;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR en GetAllAsync: {ex.Message}\n{ex.StackTrace}");
            throw;
        }
    }

    public async Task<IEnumerable<HoseViewDto>> GetHosesFromViewAsync()
    {
        try
        {
            using var context = dbContextFactory.CreateDbContext();
            var hoses = new List<HoseViewDto>();
            using var connection = context.Database.GetDbConnection();
            await connection.OpenAsync();

            string query = "SELECT * FROM v_hose_list";
            using var command = connection.CreateCommand();
            command.CommandText = query;

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                hoses.Add(new HoseViewDto
                {
                    IdHose = reader.GetInt32(reader.GetOrdinal("id_hose")),
                    HoseNumber = reader.GetInt32(reader.GetOrdinal("hose_number")),
                    Dispenser = reader.GetInt32(reader.GetOrdinal("dispenser")),
                    AccumulatedGallons = reader.GetDouble(reader.GetOrdinal("accumulated_gallons")),
                    AccumulatedAmount = reader.GetDouble(reader.GetOrdinal("accumulated_amount")),
                    ProductType = reader.GetInt32(reader.GetOrdinal("product_type")),
                    Price = reader.GetDouble(reader.GetOrdinal("price")),

                    IdDispenser = reader.GetInt32(reader.GetOrdinal("id_dispensers")),
                    Code = reader.GetString(reader.GetOrdinal("code")),
                    Number = reader.GetInt32(reader.GetOrdinal("number")),
                    IdDispenserType = reader.GetInt32(reader.GetOrdinal("id_dispenser_type")),
                    IdEds = reader.GetInt32(reader.GetOrdinal("id_eds")),
                    IdIsland = reader.GetInt32(reader.GetOrdinal("idisland")),
                    NumberHose = reader.GetInt32(reader.GetOrdinal("number_hose")),

                    IdProductType = reader.GetInt32(reader.GetOrdinal("id_product_type")),
                    Description = reader.GetString(reader.GetOrdinal("description")),

                    EdsId = reader.GetInt32(reader.GetOrdinal("dispensers")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Nit = reader.GetString(reader.GetOrdinal("nit")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    Sicom = reader.GetString(reader.GetOrdinal("sicom")),
                    IdBusiness = reader.GetInt32(reader.GetOrdinal("idbusiness"))
                });
            }

            return hoses;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR en GetHosesFromViewAsync: {ex.Message}\n{ex.StackTrace}");
            throw;
        }
    }
    async Task<IEnumerable<HoseDto>> IHoseGetAllHose.GetAllAsync(PaginationParams paginationParams)
    {
        var result = await GetAllAsync(paginationParams);
        return result.Select(h => new HoseDto(
            h.IdHose,
            h.Number,
            h.IdDispenser,
            h.AccumulatedGallons,
            h.AccumulatedAmount,
            h.IdProductType,
            h.Price,
            new DispensersEntity(), 
            new ProductTypeEntity(),
            new EdsEntity()
        ));
    }


}
