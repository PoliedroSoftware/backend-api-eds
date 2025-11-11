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

            // Save changes - EF will only update if there are actual changes
            await context.SaveChangesAsync();
            
            // Clear cache after successful update
            await redisService.RemoveByPrefixAsync("compartiment:");

            return VoidResult.Instance;
        }
        catch (DbUpdateException dbEx)
        {
            // Log the full exception server-side for debugging
            Console.WriteLine($"[ERROR] Database update failed for compartiment {compartimentEntity.IdCompartiment}: {dbEx}");
            
            // Check for common database issues
            var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
            if (innerException.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
                innerException.Contains("FK_", StringComparison.OrdinalIgnoreCase))
            {
                return Error.CreateInstance(
                    "ForeignKeyConstraintViolation",
                    "The update failed due to invalid references. Please verify that IdProduct and IdTank reference existing records.",
                    System.Net.HttpStatusCode.BadRequest);
            }
            
            if (innerException.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) ||
                innerException.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return Error.CreateInstance(
                    "UniqueConstraintViolation",
                    "The update failed due to a duplicate value constraint. Please verify the data is unique.",
                    System.Net.HttpStatusCode.Conflict);
            }
            
            return Error.CreateInstance(
                "DatabaseError",
                "Failed to update the compartiment due to a database error. Please contact support if the problem persists.",
                System.Net.HttpStatusCode.InternalServerError);
        }
        catch (Exception ex)
        {
            // Log the full exception server-side for debugging
            Console.WriteLine($"[ERROR] Unexpected error updating compartiment {compartimentEntity.IdCompartiment}: {ex}");
            return Error.CreateInstance(
                "CompartimentUpdateError",
                "An unexpected error occurred while updating the compartiment. Please contact support if the problem persists.",
                System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
