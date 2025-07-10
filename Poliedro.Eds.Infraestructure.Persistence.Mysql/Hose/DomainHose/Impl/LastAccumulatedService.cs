using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl;

public class LastAccumulatedService(
    ITenantDbContextFactory dbContextFactory,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor
    , IConfiguration config) : ILastAccumulatedService
{
    public async Task<Result<LastAccumulatedEntity, Error>> GetLastAccumulatedAsync(int idDispenser, int idHose)
    {
        LastAccumulatedEntity? lastAccumulated = null;
        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
        string cacheKey = $"LastAccumulated:{idDispenser}:{idHose}:{tenant}";
        var cachedData = await redisService.GetCacheAsync<LastAccumulatedEntity>(cacheKey);

        if (cachedData is not null)
            return cachedData;

        using var context = dbContextFactory.CreateDbContext();
        using var connection = context.Database.GetDbConnection();


        //using (MySqlConnection connection = new(config["ConnectionStrings:MysqlConnection"]))
        {
            try
            {
                await connection.OpenAsync();
                string query = "SELECT accumulated_amount, accumulatd_gallons FROM hose_history WHERE id_dispensers = @IdDispenser AND id_hose = @IdHose ORDER BY date DESC LIMIT 1";

                using var command = connection.CreateCommand();
                command.CommandText = query;

                var idDispenserParam = command.CreateParameter();
                idDispenserParam.ParameterName = "@IdDispenser";
                idDispenserParam.Value = idDispenser;
                command.Parameters.Add(idDispenserParam);

                var idHoseParam = command.CreateParameter();
                idHoseParam.ParameterName = "@IdHose";
                idHoseParam.Value = idHose;
                command.Parameters.Add(idHoseParam);

                using var reader = await command.ExecuteReaderAsync();

                //using MySqlCommand command = new(query, connection);
                //command.Parameters.AddWithValue("@IdDispenser", idDispenser);
                //command.Parameters.AddWithValue("@IdHose", idHose);

                //using MySqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    lastAccumulated = new LastAccumulatedEntity
                    {
                        LastAccumulatedAmount = reader.GetDouble(0),
                        LastAccumulatedGallons = reader.GetDouble(1),

                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            await redisService.SetCacheAsync(cacheKey, lastAccumulated, TimeSpan.FromMinutes(1440));
        }
        return Result<LastAccumulatedEntity, Error>.Success(lastAccumulated);
    }
}