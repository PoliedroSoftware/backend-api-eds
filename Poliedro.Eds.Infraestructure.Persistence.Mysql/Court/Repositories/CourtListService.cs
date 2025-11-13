using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

public class CourtListService(
    ITenantDbContextFactory dbContextFactory
    ) : ICourtListDomainService
{
    public async Task<IEnumerable<CourtListResponseEntity>> GetAllAsync(PaginationParams paginationParams, string username, bool isAdmin)
    {
        try
        {
            var pagedCourts = await GetCourtsFromViewAsync(username, isAdmin, paginationParams);

            if (!pagedCourts.Any())
            {
                return Enumerable.Empty<CourtListResponseEntity>();
            }
            var courtIds = pagedCourts.Select(c => c.Id).ToList();
            var collections = await GetCourtCollectionsFromViewAsync(courtIds);
            var dispensers = await GetCourtDispensersFromViewAsync(courtIds);
            var documents = await GetCourtDocumentsFromViewAsync(courtIds);
            var expenditures = await GetCourtExpendituresFromViewAsync(courtIds);
            var result = pagedCourts
                .Select(court => new CourtListResponseEntity
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
                })
                .ToList();

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en CourtListService.GetAllAsync: {ex.Message}", ex);
        }
    }

    private async Task<List<CourtViewEntity>> GetCourtsFromViewAsync(
        string username,
        bool isAdmin,
        PaginationParams paginationParams)
    {
        using var context = dbContextFactory.CreateDbContext();
        var courts = new List<CourtViewEntity>();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        string query;
        int offset = (paginationParams.PageNumber - 1) * paginationParams.PageSize;
        int limit = paginationParams.PageSize;

        using var command = connection.CreateCommand();

        if (isAdmin)
        {
            query = "SELECT * FROM v_court ORDER BY id DESC LIMIT @limit OFFSET @offset";
            command.CommandText = query;
        }
        else
        {
            query = "SELECT * FROM v_court WHERE islander = @islanderName ORDER BY id DESC LIMIT @limit OFFSET @offset";
            command.CommandText = query;

            var paramIslander = command.CreateParameter();
            paramIslander.ParameterName = "@islanderName";
            paramIslander.Value = username;
            command.Parameters.Add(paramIslander);
        }

        var paramLimit = command.CreateParameter();
        paramLimit.ParameterName = "@limit";
        paramLimit.Value = limit;
        command.Parameters.Add(paramLimit);

        var paramOffset = command.CreateParameter();
        paramOffset.ParameterName = "@offset";
        paramOffset.Value = offset;
        command.Parameters.Add(paramOffset);

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courts.Add(new CourtViewEntity
            {
                Id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                Consecutive = reader.IsDBNull("consecutive") ? 0 : reader.GetInt32("consecutive"),
                IdEds = reader.IsDBNull("id_eds") ? 0 : reader.GetInt32("id_eds"),
                Eds = reader.IsDBNull("eds") ? string.Empty : reader.GetString("eds"),
                Bussiness = reader.IsDBNull("bussiness") ? string.Empty : reader.GetString("bussiness"),
                Islander = reader.IsDBNull("islander") ? string.Empty : reader.GetString("islander"),
                DateStarttime = reader.IsDBNull("date_starttime") ? default : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_starttime"))),
                Starttime = reader.IsDBNull("starttime") ? default : TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(reader.GetOrdinal("starttime"))),
                DateEndtime = reader.IsDBNull("date_endtime") ? default : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_endtime"))),
                Endtime = reader.IsDBNull("endtime") ? default : TimeOnly.FromTimeSpan((TimeSpan)reader.GetValue(reader.GetOrdinal("endtime"))),
                Distinc = reader.IsDBNull("distinc") ? 0.0 : reader.GetDouble("distinc"),
                TotalAccumulatedAmount = reader.IsDBNull("total_accumulated_amount") ? 0.0 : reader.GetDouble("total_accumulated_amount"),
                TotalAccumulatedGallons = reader.IsDBNull("total_accumulated_gallons") ? 0.0 : reader.GetDouble("total_accumulated_gallons")
            });
        }

        return courts;
    }

    private async Task<IEnumerable<CourtCollectionViewEntity>> GetCourtCollectionsFromViewAsync(List<int> courtIds)
    {
        if (!courtIds.Any())
        {
            return Enumerable.Empty<CourtCollectionViewEntity>();
        }

        using var context = dbContextFactory.CreateDbContext();
        var collections = new List<CourtCollectionViewEntity>();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        string courtIdsString = string.Join(",", courtIds);
        string query = $"SELECT * FROM v_court_collection WHERE court IN ({courtIdsString}) ORDER BY court DESC";

        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            collections.Add(new CourtCollectionViewEntity
            {
                Id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                Court = reader.IsDBNull("court") ? 0 : reader.GetInt32("court"),
                Date = reader.IsDBNull("date") ? default : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date"))),
                Collection = reader.IsDBNull("collection") ? string.Empty : reader.GetString("collection"),
                Amount = reader.IsDBNull("amount") ? 0 : reader.GetDouble("amount"),
                Description = reader.IsDBNull("description") ? string.Empty : reader.GetString("description")
            });
        }

        return collections;
    }

    private async Task<IEnumerable<CourtDispenserViewEntity>> GetCourtDispensersFromViewAsync(List<int> courtIds)
    {
        if (!courtIds.Any())
        {
            return Enumerable.Empty<CourtDispenserViewEntity>();
        }

        var dispensers = new List<CourtDispenserViewEntity>();

        using var context = dbContextFactory.CreateDbContext();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        string courtIdsString = string.Join(",", courtIds);
        string query = $"SELECT * FROM v_court_dispenser WHERE code_court IN ({courtIdsString}) ORDER BY code_court DESC";

        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            dispensers.Add(new CourtDispenserViewEntity
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
                DateStarttime = reader.IsDBNull("date_starttime") ? default : DateOnly.FromDateTime(reader.GetDateTime("date_starttime")),
                DateEndtime = reader.IsDBNull("date_endtime") ? default : DateOnly.FromDateTime(reader.GetDateTime("date_endtime")),
                Distinc = reader.IsDBNull("distinc") ? 0.0 : reader.GetDouble("distinc"),
                Product = reader.IsDBNull("product") ? string.Empty : reader.GetString("product"),
                PurchasePrice = reader.IsDBNull("purchase_price") ? 0.0 : reader.GetDouble("purchase_price"),
                SellPrice = reader.IsDBNull("sell_price") ? 0.0 : reader.GetDouble("sell_price"),
                ProductType = reader.IsDBNull("product_typr") ? string.Empty : reader.GetString("product_typr"),
                AccumulatedAmount = reader.IsDBNull("accumulated_amount") ? 0.0 : reader.GetDouble("accumulated_amount"),
                AccumulatedGallons = reader.IsDBNull("accumulated_gallons") ? 0.0 : reader.GetDouble("accumulated_gallons")
            });
        }

        return dispensers;
    }

    private async Task<IEnumerable<CourtDocumentViewEntity>> GetCourtDocumentsFromViewAsync(List<int> courtIds)
    {
        if (!courtIds.Any())
        {
            return Enumerable.Empty<CourtDocumentViewEntity>();
        }

        var documents = new List<CourtDocumentViewEntity>();
        using var context = dbContextFactory.CreateDbContext();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        string courtIdsString = string.Join(",", courtIds);
        string query = $"SELECT * FROM v_court_document WHERE court IN ({courtIdsString}) ORDER BY court DESC";

        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            documents.Add(new CourtDocumentViewEntity
            {
                Id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                Court = reader.IsDBNull("court") ? 0 : reader.GetInt32("court"),
                Descripcion = reader.IsDBNull("descripcion") ? string.Empty : reader.GetString("descripcion"),
                DocumentName = reader.IsDBNull("document_name") ? null : reader.GetString("document_name")
            });
        }

        return documents;
    }


    private async Task<IEnumerable<CourtExpenditureViewEntity>> GetCourtExpendituresFromViewAsync(List<int> courtIds)
    {
        if (!courtIds.Any())
        {
            return Enumerable.Empty<CourtExpenditureViewEntity>();
        }

        var expenditures = new List<CourtExpenditureViewEntity>();
        using var context = dbContextFactory.CreateDbContext();
        using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        string courtIdsString = string.Join(",", courtIds);
        string query = $"SELECT * FROM v_court_expenditure WHERE court IN ({courtIdsString}) ORDER BY court DESC";

        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenditures.Add(new CourtExpenditureViewEntity
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
