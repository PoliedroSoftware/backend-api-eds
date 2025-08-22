using System.Net;
using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.OpenAI.DomainOpenAI;
using Poliedro.Eds.Domain.OpenAI.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.OpenAI.Repositories;

public class OpenAIResponseRepository(ITenantDbContextFactory dbContextFactory) : IOpenAIResponseRepository
{
    public async Task<Result<OpenAIResponseEntity, Error>> CreateAsync(OpenAIResponseEntity response)
    {
        try
        {
            using var context = dbContextFactory.CreateDbContext();
            context.Set<OpenAIResponseEntity>().Add(response);
            await context.SaveChangesAsync();
            return Result<OpenAIResponseEntity, Error>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<OpenAIResponseEntity, Error>.Failure(
                Error.CreateInstance("DatabaseError", ex.Message, HttpStatusCode.InternalServerError));
        }
    }

    public async Task<Result<IEnumerable<OpenAIResponseEntity>, Error>> GetByUserIdAsync(string userId, int pageNumber, int pageSize)
    {
        try
        {
            using var context = dbContextFactory.CreateDbContext();
            
            var responses = await context.Set<OpenAIResponseEntity>()
                .Include(r => r.Request)
                .Where(r => r.Request.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Result<IEnumerable<OpenAIResponseEntity>, Error>.Success(responses);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<OpenAIResponseEntity>, Error>.Failure(
                Error.CreateInstance("DatabaseError", ex.Message, HttpStatusCode.InternalServerError));
        }
    }
}