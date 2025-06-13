using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Court.DomainService;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

public class CourtListService(IConfiguration config, IRedisService redisService) : ICourtListDomainService
{
    private readonly string _connectionString = config["ConnectionStrings:MysqlConnection"];

    public async Task<IEnumerable<CourtListResponseDto>> GetAllAsync(PaginationParams paginationParams)
    {
        string cachekey = $"coutListService:{paginationParams.PageNumber}:{paginationParams.PageSize}";
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
        var courts = new List<CourtViewDto>();
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT * FROM v_court";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courts.Add(new CourtViewDto
            {
                Id = reader.GetInt32("id"),
                Consecutive = reader.GetInt32("consecutive"),
                IdEds = reader.GetInt32("id_eds"),
                Eds = reader.GetString("eds"),
                Bussiness = reader.GetString("bussiness"),
                Islander = reader.GetString("islander"),
                DateStarttime = reader.GetDateOnly("date_starttime"),
                Starttime = reader.GetTimeOnly("starttime"),
                DateEndtime = reader.GetDateOnly("date_endtime"),
                Endtime = reader.GetTimeOnly("endtime"),
                Distinc = reader.GetDouble("distinc"),
                TotalAccumulatedAmount = reader.GetDouble("total_accumulated_amount"),
                TotalAccumulatedGallons = reader.GetDouble("total_accumulated_gallons")
            });
    }

        return courts;
    }

    private async Task<IEnumerable<CourtCollectionViewDto>> GetCourtCollectionsFromViewAsync()
    {
        var collections = new List<CourtCollectionViewDto>();
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT * FROM v_court_collection";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            collections.Add(new CourtCollectionViewDto
            {
                Id = reader.GetInt32("id"),
                Court = reader.GetInt32("court"),
                Date = reader.GetDateOnly("date"),
                Collection = reader.GetString("collection"),
                Amount = reader.GetDouble("amount"),
                Description = reader.GetString("description")
            });
        }

        return collections;
    }

    private async Task<IEnumerable<CourtDispenserViewDto>> GetCourtDispensersFromViewAsync()
    {
        var dispensers = new List<CourtDispenserViewDto>();
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT * FROM v_court_dispenser";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            dispensers.Add(new CourtDispenserViewDto
            {
                Id = reader.GetInt32("id"),
                Business = reader.GetString("business"),
                IdEds = reader.GetInt32("id_eds"),
                Eds = reader.GetString("eds"),
                Dispenser = reader.GetInt32("dispenser"),
                NumberHose = reader.GetInt32("number_hose"),
                LastAccumulatedAmount = reader.GetDouble("last_accumulated_amount"),
                LastAccumulatedGallons = reader.GetDouble("last_accumulated_gallons"),
                CodeCourt = reader.GetInt32("code_court"),
                Islander = reader.GetString("islander"),
                DateStarttime = reader.GetDateOnly("date_starttime"),
                Starttime = reader.GetTimeOnly("starttime"),
                DateEndtime= reader.GetDateOnly("date_endtime"),
                Endtime = reader.GetTimeOnly("endtime"),                
                Distinc = reader.GetDouble("distinc"),
                Product = reader.GetString("product"),
                Price = reader.GetDouble("price"),
                ProductType = reader.GetString("product_typr"),
                AccumulatedAmount = reader.GetDouble("accumulated_amount"),
                AccumulatedGallons = reader.GetDouble("accumulated_gallons")
            });
        }

        return dispensers;
    }

    private async Task<IEnumerable<CourtDocumentViewDto>> GetCourtDocumentsFromViewAsync()
    {
        var documents = new List<CourtDocumentViewDto>();
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT * FROM v_court_document";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            documents.Add(new CourtDocumentViewDto
            {
                Id = reader.GetInt32("id"),
                Court = reader.GetInt32("court"),
                Descripcion = reader.GetString("descripcion")
            });
        }

        return documents;
    }

    private async Task<IEnumerable<CourtExpenditureViewDto>> GetCourtExpendituresFromViewAsync()
    {
        var expenditures = new List<CourtExpenditureViewDto>();
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT * FROM v_court_expenditure";
        using var command = new MySqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenditures.Add(new CourtExpenditureViewDto
            {
                Id = reader.GetInt32("id"),
                Court = reader.GetInt32("court"),
                Date = reader.GetDateOnly("date"),
                Expenditure = reader.GetString("expenditure"),
                Amount = reader.GetDouble("amount"),
                Description = reader.GetString("description")
            });
        }

        return expenditures;
    }
}
