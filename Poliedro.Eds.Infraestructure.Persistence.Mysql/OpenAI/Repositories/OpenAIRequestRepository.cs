using System.Net;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.OpenAI.DomainOpenAI;
using Poliedro.Eds.Domain.OpenAI.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.OpenAI.Repositories;

public class OpenAIRequestRepository(ITenantDbContextFactory dbContextFactory) : IOpenAIRequestRepository
{
    public async Task<Result<OpenAIRequestEntity, Error>> CreateAsync(OpenAIRequestEntity request)
    {
        try
        {
            using var context = dbContextFactory.CreateDbContext();
            context.Set<OpenAIRequestEntity>().Add(request);
            await context.SaveChangesAsync();
            return Result<OpenAIRequestEntity, Error>.Success(request);
        }
        catch (Exception ex)
        {
            return Result<OpenAIRequestEntity, Error>.Failure(
                Error.CreateInstance("DatabaseError", ex.Message, HttpStatusCode.InternalServerError));
        }
    }

    public async Task<Result<IEnumerable<OpenAIRequestEntity>, Error>> GetByUserIdAsync(string userId, int pageNumber, int pageSize)
    {
        try
        {
            using var context = dbContextFactory.CreateDbContext();
            
            var requests = await context.Set<OpenAIRequestEntity>()
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Result<IEnumerable<OpenAIRequestEntity>, Error>.Success(requests);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<OpenAIRequestEntity>, Error>.Failure(
                Error.CreateInstance("DatabaseError", ex.Message, HttpStatusCode.InternalServerError));
        }
    }
}