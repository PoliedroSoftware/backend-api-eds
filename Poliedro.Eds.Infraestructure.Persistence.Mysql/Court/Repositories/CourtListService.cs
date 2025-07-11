using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using System.Data;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

public class CourtListService(IConfiguration config, 
    IRedisService redisService, 
    ITenantDbContextFactory dbContextFactory,
    IHttpContextAccessor httpContextAccessor) : ICourtListDomainService
{
    public async Task<IEnumerable<CourtListResponseDto>> GetAllAsync(PaginationParams paginationParams)
    {
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cachekey = $"coutListService:{paginationParams.PageNumber}:{paginationParams.PageSize}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<IEnumerable<CourtListResponseDto>>(cachekey);
        
        if (cachedData != null) return cachedData;
        try
        {
            var courts = await GetCourtsFromViewAsync();
            var collections = await GetCourtCollectionsFromViewAsync();
            var dispensers = await GetCourtDispensersFromViewAsync();
            var documents = await GetCourtDocumentsFromViewAsync();
            var expenditures = await GetCourtExpendituresFromViewAsync();

            var groupedCourts = courts.Select(court => new CourtListResponseDto
            {
                Id = court.Id,
                Consecutive = court.Consecutive,
                IdEds = court.IdEds,
                Eds = court.Eds,
                Bussiness = court.Bussiness,
                Islander = court.Islander,
                DateStarttime = court.DateStarttime,
                Starttime = court.Starttime,
                DateEndtime = court.DateEndtime,
                Endtime = court.Endtime,
                Distinc = court.Distinc,
                TotalAccumulatedAmount = court.TotalAccumulatedAmount,
                TotalAccumulatedGallons = court.TotalAccumulatedGallons,
                Collections = collections.Where(x => x.Court == court.Id).ToList(),
                Dispensers = dispensers.Where(x => x.CodeCourt == court.Id).ToList(),
                Documents = documents.Where(x => x.Court == court.Id).ToList(),
                Expenditures = expenditures.Where(x => x.Court == court.Id).ToList()
            });

            var pagedCourts = groupedCourts
                .OrderByDescending(c => c.DateStarttime)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            await redisService.SetCacheAsync(cachekey, pagedCourts, TimeSpan.FromMinutes(1440));
            return pagedCourts;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en CourtListService.GetAllAsync: {ex.Message}", ex);
        }
    }

    private async Task<IEnumerable<CourtViewDto>> GetCourtsFromViewAsync()
    {
        using var context = dbContextFactory.CreateDbContext();
        var courts = new List<CourtViewDto>();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        string query = "SELECT * FROM v_court";
        using var command = connection.CreateCommand();
        command.CommandText = query;

        
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courts.Add(new CourtViewDto
            {
                Id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                Consecutive = reader.IsDBNull("consecutive") ? 0 : reader.GetInt32("consecutive"),
                IdEds = reader.IsDBNull("id_eds") ? 0 : reader.GetInt32("id_eds"),
                Eds = reader.IsDBNull("eds") ? null : reader.GetString("eds"),
                Bussiness = reader.IsDBNull("bussiness") ? string.Empty : reader.GetString("bussiness"),
                Islander = reader.IsDBNull("islander") ? string.Empty : reader.GetString("islander"),
                DateStarttime = reader.IsDBNull("date_starttime") ? default: DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_starttime"))),
                //Starttime = reader.IsDBNull("starttime")? default:TimeOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("starttime"))),
                DateEndtime = reader.IsDBNull("date_endtime")? default: DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_endtime"))),
                //Endtime = reader.IsDBNull("endtime")? default: TimeOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("endtime"))),
                Distinc = reader.IsDBNull("distinc") ? 0.0 : reader.GetDouble("distinc"),
                TotalAccumulatedAmount = reader.IsDBNull("total_accumulated_amount") ? 0.0 : reader.GetDouble("total_accumulated_amount"),
                TotalAccumulatedGallons = reader.IsDBNull("total_accumulated_gallons") ? 0.0 : reader.GetDouble("total_accumulated_gallons")
            });
        }

        return courts;
    }

    private async Task<IEnumerable<CourtCollectionViewDto>> GetCourtCollectionsFromViewAsync()
    {
        using var context = dbContextFactory.CreateDbContext();
        var collections = new List<CourtCollectionViewDto>();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();
        string query = "SELECT * FROM v_court_collection";
        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            collections.Add(new CourtCollectionViewDto
            {
                Id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                Court = reader.IsDBNull("court") ? 0 : reader.GetInt32("court"),
                Date = reader.IsDBNull("date") ? default: DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date"))),
                Collection = reader.IsDBNull("collection") ? string.Empty : reader.GetString("collection"),
                Amount = reader.IsDBNull("amount") ? 0 : reader.GetDouble("amount"),
                Description = reader.IsDBNull("description") ? string.Empty : reader.GetString("description")
            });
        }

        return collections;
    }

    private async Task<IEnumerable<CourtDispenserViewDto>> GetCourtDispensersFromViewAsync()
    {
        var dispensers = new List<CourtDispenserViewDto>();

        using var context = dbContextFactory.CreateDbContext();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();
        string query = "SELECT * FROM v_court_dispenser";
        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            dispensers.Add(new CourtDispenserViewDto
            {
                Id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                Business = reader.IsDBNull("business") ? string.Empty : reader.GetString("business"),
                IdEds = reader.IsDBNull("id_eds") ? 0 : reader.GetInt32("id_eds"),
                Eds = reader.IsDBNull("eds") ? string.Empty : reader.GetString("eds"),
                Dispenser = reader.IsDBNull("dispenser") ? 0 : reader.GetInt32("dispenser"),
                NumberHose = reader.IsDBNull("number_hose") ? 0 : reader.GetInt32("number_hose"),
                LastAccumulatedAmount = reader.IsDBNull("last_accumulated_amount") ? 0.0 : reader.GetDouble("last_accumulated_amount"),
                LastAccumulatedGallons = reader.IsDBNull("last_accumulated_gallons") ? 0.0 : reader.GetDouble("last_accumulated_gallons"),
                CodeCourt = reader.IsDBNull("code_court") ? 0 : reader.GetInt32("code_court"),
                Islander = reader.IsDBNull("islander") ? string.Empty : reader.GetString("islander"),
                DateStarttime =  reader.IsDBNull("date_starttime") ? default: DateOnly.FromDateTime(reader.GetDateTime("date_starttime")),
                Starttime = reader.IsDBNull("starttime") ? default : TimeOnly.FromDateTime(reader.GetDateTime("starttime")),
                DateEndtime = reader.IsDBNull("date_endtime") ? default : DateOnly.FromDateTime(reader.GetDateTime("date_endtime")),
                Endtime = reader.IsDBNull("endtime") ? default: TimeOnly.FromDateTime(reader.GetDateTime("endtime")),
                Distinc = reader.IsDBNull("distinc") ? 0.0 : reader.GetDouble("distinc"),
                Product = reader.IsDBNull("product") ? string.Empty : reader.GetString("product"),
                Price = reader.IsDBNull("price") ? 0.0 : reader.GetDouble("price"),
                ProductType = reader.IsDBNull("product_typr") ? string.Empty : reader.GetString("product_typr"),
                AccumulatedAmount = reader.IsDBNull("accumulated_amount") ? 0.0 : reader.GetDouble("accumulated_amount"),
                AccumulatedGallons = reader.IsDBNull("accumulated_gallons") ? 0.0 : reader.GetDouble("accumulated_gallons")
            });
        }

        return dispensers;
    }

    private async Task<IEnumerable<CourtDocumentViewDto>> GetCourtDocumentsFromViewAsync()
    {
        var documents = new List<CourtDocumentViewDto>();
        using var context = dbContextFactory.CreateDbContext();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        string query = "SELECT * FROM v_court_document";
        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            documents.Add(new CourtDocumentViewDto
            {
                Id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                Court = reader.IsDBNull("court") ? 0 : reader.GetInt32("court"),
                Descripcion = reader.IsDBNull("descripcion") ? string.Empty : reader.GetString("descripcion")
            });
        }

        return documents;
    }

    private async Task<IEnumerable<CourtExpenditureViewDto>> GetCourtExpendituresFromViewAsync()
    {
        var expenditures = new List<CourtExpenditureViewDto>();
        using var context = dbContextFactory.CreateDbContext();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        string query = "SELECT * FROM v_court_expenditure";
        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenditures.Add(new CourtExpenditureViewDto
            {
                Id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                Court = reader.IsDBNull("court") ? 0 : reader.GetInt32("court"),
                Date = reader.IsDBNull("date") ? default : DateOnly.FromDateTime(reader.GetDateTime("date")),
                Expenditure = reader.GetString("expenditure"),
                Amount = reader.IsDBNull("amount") ? 0.0 : reader.GetDouble("amount"),
                Description = reader.IsDBNull("description") ? string.Empty : reader.GetString("description")
            });
        }

        return expenditures;
    }
}
