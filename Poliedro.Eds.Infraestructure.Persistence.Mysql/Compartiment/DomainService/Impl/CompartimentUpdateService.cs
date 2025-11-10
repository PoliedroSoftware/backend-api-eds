using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.Compartiment.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Compartiment.DomainCompartiment.Impl;

public class CompartimentUpdateService(ITenantDbContextFactory dbContextFactory, IRedisService redisService) : ICompartimentUpdateService
{
    public async Task<Result<VoidResult, Error>> UpdateAsync(CompartimentEntity compartimentEntity)
    {
        try
        {
            using var context = dbContextFactory.CreateDbContext();
            
            var existingEntity = await context.Compartiment
                .FirstOrDefaultAsync(c => c.IdCompartiment == compartimentEntity.IdCompartiment);

            if (existingEntity is null)
                return CompartimentErrorBuilder.CompartimentNotFoundException(compartimentEntity.IdCompartiment);

            // Update only the fields provided in the command, preserving Date and audit fields
            existingEntity.Number = compartimentEntity.Number;
            existingEntity.Nominal = compartimentEntity.Nominal;
            existingEntity.Operative = compartimentEntity.Operative;
            existingEntity.IdProduct = compartimentEntity.IdProduct;
            existingEntity.Height = compartimentEntity.Height;
            existingEntity.IdTank = compartimentEntity.IdTank;

            if (await context.SaveChangesAsync() <= 0)
                return CompartimentErrorBuilder.CompartimentUpdateException();
            await redisService.RemoveByPrefixAsync("compartiment:");

            return VoidResult.Instance;
        }
        catch (Exception ex)
        {
            return Error.CreateInstance(
                "CompartimentUpdateError",
                $"Failed to update compartiment due to an error: {ex.Message}",
                System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
