using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.DomainServices.GetByNumber;
using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Phone.DomainService.Impl;

public class PhoneGetByNumberService(
    ITenantDbContextFactory dbContextFactory,
    ILogger<PhoneGetByNumberService> logger) : IPhoneGetByNumberService
{
    public async Task<Result<PhoneEntity?, Error>> GetByNumberAsync(string number)
    {
        logger.LogInformation("Getting phone by number: {PhoneNumber}", number);
        
        using var context = dbContextFactory.CreateDbContext();
        var phone = await context.Phone
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Number == number);
        
        if (phone != null)
        {
            logger.LogInformation("Phone found: {PhoneNumber}", number);
        }
        else
        {
            logger.LogInformation("Phone not found: {PhoneNumber}", number);
        }
        
        return Result<PhoneEntity?, Error>.Success(phone);
    }

    public async Task<bool> ExistsAsync(string number)
    {
        logger.LogInformation("Checking if phone exists: {PhoneNumber}", number);
        
        using var context = dbContextFactory.CreateDbContext();
        var exists = await context.Phone
            .AsNoTracking()
            .AnyAsync(p => p.Number == number);
        
        logger.LogInformation("Phone existence check for {PhoneNumber}: {Exists}", number, exists);
        return exists;
    }
}
