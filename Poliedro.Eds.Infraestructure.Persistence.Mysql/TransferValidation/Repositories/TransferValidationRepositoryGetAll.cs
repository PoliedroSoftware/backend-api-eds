using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.TransferValidation.Entities;
using Poliedro.Eds.Domain.TransferValidation.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.TransferValidation.Repositories;

public class TransferValidationRepositoryGetAll(
  ITenantDbContextFactory dbContextFactory,
    ILogger<TransferValidationRepositoryGetAll> logger)
    : ITransferValidationRepositoryGetAll
{
    public async Task<IEnumerable<TransferValidationEntity>> GetAllAsync(
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Obteniendo todas las validaciones de transferencia - Página: {PageNumber}, Tamaño: {PageSize}",
         paginationParams.PageNumber, paginationParams.PageSize);

        using var context = dbContextFactory.CreateDbContext();

        var data = await context.TransferValidation
            .OrderByDescending(t => t.CreatedAt)
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync(cancellationToken);

        logger.LogInformation("Se encontraron {Count} validaciones de transferencia", data.Count);

        return data;
    }
}
