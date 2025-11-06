using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.DomainServices.GetByNumber;
using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Phone.DomainService.Impl;

public class PhoneGetByNumberService(ITenantDbContextFactory dbContextFactory) : IPhoneGetByNumberService
{
    public async Task<Result<PhoneEntity?, Error>> GetByNumberAsync(string number)
    {
        using var context = dbContextFactory.CreateDbContext();
        var phone = await context.Phone
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Number == number);
        
        return Result<PhoneEntity?, Error>.Success(phone);
    }

    public async Task<bool> ExistsAsync(string number)
    {
        using var context = dbContextFactory.CreateDbContext();
        return await context.Phone
            .AsNoTracking()
            .AnyAsync(p => p.Number == number);
    }
}
