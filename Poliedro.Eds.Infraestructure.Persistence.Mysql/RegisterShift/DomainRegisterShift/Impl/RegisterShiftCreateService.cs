using Microsoft.EntityFrameworkCore.Internal;
using Poliedro.Eds.Application.RegisterShift.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.RegisterShift.Validations;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.RegisterShift.DomainService;
using Poliedro.Eds.Domain.RegisterShift.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Microsoft.Extensions.Logging;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.RegisterShift.DomainRegisterShift.Impl;

public class RegisterShiftCreateService(ITenantDbContextFactory dbContextFactory, ILogger<RegisterShiftCreateService> logger) : IRegisterShiftCreateService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(RegisterShiftEntity entity)
    {
        try
        {
            using var context = dbContextFactory.CreateDbContext();
            await context.RegisterShift.AddAsync(entity);
            var saved = await context.SaveChangesAsync() > 0;
            if (!saved)
            {
                return RegisterShiftBuilder.RegisterShiftCreationException();
            }
            else
            {
                logger.LogInformation("Successfully created RegisterShift");
return VoidResult.Instance;
            }
        }
        catch (Exception)
        {
            // Mapear cualquier excepción de persistencia a un Error de dominio controlado
            return RegisterShiftBuilder.RegisterShiftCreationException();
        }
    }
}
