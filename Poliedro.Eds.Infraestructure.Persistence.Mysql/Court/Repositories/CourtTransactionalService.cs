using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories
{
    public class CourtTransactionalService(
        ITenantDbContextFactory dbContextFactory,
        IGetProductAndCompartiment getProductAndCompartiment,
        ILogger<CourtTransactionalService> logger,
        IRedisService redisService) : ICourtTransactionalService
    {
        public async Task<Result<CourtEntity, Error>> ExecuteCourtTransactionAsync(
            CourtEntity courtEntity,
            IEnumerable<CourtDispenserSaleEntity> courtDispenserSaleEntities,
            CancellationToken cancellationToken = default)
        {
            using var context = dbContextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                logger.LogInformation("=== INICIANDO TRANSACCIÓN DEL CORTE ===");
                logger.LogInformation("Corte ID: {CourtId}, Dispensadores: {DispenserCount}, Gastos: {ExpenditureCount}, Tipos de cobro: {CollectionCount}",
                    courtEntity.IdCourt, 
                    courtEntity.CourtDispensers.Count(),
                    courtEntity.CourtExpenditures?.Count() ?? 0,
                    courtEntity.CourtTypeOfCollections.Count());

                // 1. Crear el corte
                await context.Court.AddAsync(courtEntity, cancellationToken);
                var courtSaveResult = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!courtSaveResult)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    logger.LogError("❌ ERROR: Falló la creación del corte");
                    return CourtErrorBuilder.CourtCreationException();
                }
                logger.LogInformation("✅ Corte creado exitosamente");

                // 2. Actualizar inventario de productos (reducir stock)
                if (courtDispenserSaleEntities.Any())
                {
                    var inventoryUpdateResult = await UpdateProductInventoryAsync(
                        context, courtDispenserSaleEntities, cancellationToken);
                    
                    if (!inventoryUpdateResult.IsSuccess)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        logger.LogError("❌ ERROR: Falló la actualización de inventario - {ErrorDescription}", 
                            inventoryUpdateResult.Error?.Description);
                        return Result<CourtEntity, Error>.Failure(inventoryUpdateResult.Error!);
                    }
                    logger.LogInformation("✅ Inventario actualizado exitosamente");
                }

                // 3. Confirmar la transacción
                await transaction.CommitAsync(cancellationToken);
                logger.LogInformation("✅ TRANSACCIÓN CONFIRMADA EXITOSAMENTE");

                // 4. Limpiar cache solo después de commit exitoso
                var result = Result<CourtEntity, Error>.Success(courtEntity);
                await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService,
                    KeyRedisConstants.BUSINESS,
                    KeyRedisConstants.COMPARTIMENT,
                    KeyRedisConstants.DISPENSERS,
                    KeyRedisConstants.EDS,
                    KeyRedisConstants.EXPENDITURES,
                    KeyRedisConstants.HOSE,
                    KeyRedisConstants.ISLANDER,
                    KeyRedisConstants.PRODUCT,
                    KeyRedisConstants.TRANSLATION,
                    KeyRedisConstants.TYPE_OF_COLLECTION);

                logger.LogInformation("✅ Cache limpiado");
                logger.LogInformation("==========================================");

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                logger.LogError(ex, "❌ ERROR CRÍTICO: Rollback ejecutado - {ErrorMessage}", ex.Message);
                logger.LogInformation("==========================================");
                return Result<CourtEntity, Error>.Failure(
                    Error.CreateInstance("CourtTransactionError", 
                        $"Error durante la transacción del corte: {ex.Message}", 
                        System.Net.HttpStatusCode.InternalServerError));
            }
        }

        public async Task<Result<CourtEntity, Error>> ExecuteCourtTransactionWithPriceValidationAsync(
            CourtEntity courtEntity,
            IEnumerable<CourtDispenserTransactionData> courtDispensers,
            IEnumerable<CourtDispenserSaleEntity> courtDispenserSaleEntities,
            CancellationToken cancellationToken = default)
        {
            using var context = dbContextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                logger.LogInformation("=== INICIANDO TRANSACCIÓN COMPLETA DEL CORTE ===");

                // 1. Validar y actualizar precios de productos
                var priceValidationResult = await ValidateAndUpdateProductPricesWithinTransactionAsync(
                    context, courtDispensers, cancellationToken);

                if (!priceValidationResult.IsSuccess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    logger.LogError("❌ ERROR: Falló la validación de precios - {ErrorDescription}", 
                        priceValidationResult.Error?.Description);
                    return Result<CourtEntity, Error>.Failure(priceValidationResult.Error!);
                }
                logger.LogInformation("✅ Precios validados y actualizados");

                // 2. Crear el corte
                await context.Court.AddAsync(courtEntity, cancellationToken);
                var courtSaveResult = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!courtSaveResult)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    logger.LogError("❌ ERROR: Falló la creación del corte");
                    return CourtErrorBuilder.CourtCreationException();
                }
                logger.LogInformation("✅ Corte creado exitosamente");

                // 3. Actualizar inventario de productos (reducir stock)
                if (courtDispenserSaleEntities.Any())
                {
                    var inventoryUpdateResult = await UpdateProductInventoryAsync(
                        context, courtDispenserSaleEntities, cancellationToken);
                    
                    if (!inventoryUpdateResult.IsSuccess)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        logger.LogError("❌ ERROR: Falló la actualización de inventario - {ErrorDescription}", 
                            inventoryUpdateResult.Error?.Description);
                        return Result<CourtEntity, Error>.Failure(inventoryUpdateResult.Error!);
                    }
                    logger.LogInformation("✅ Inventario actualizado exitosamente");
                }

                // 4. Confirmar la transacción
                await transaction.CommitAsync(cancellationToken);
                logger.LogInformation("✅ TRANSACCIÓN COMPLETA CONFIRMADA EXITOSAMENTE");

                // 5. Limpiar cache solo después de commit exitoso
                var result = Result<CourtEntity, Error>.Success(courtEntity);
                await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService,
                    KeyRedisConstants.BUSINESS,
                    KeyRedisConstants.COMPARTIMENT,
                    KeyRedisConstants.DISPENSERS,
                    KeyRedisConstants.EDS,
                    KeyRedisConstants.EXPENDITURES,
                    KeyRedisConstants.HOSE,
                    KeyRedisConstants.ISLANDER,
                    KeyRedisConstants.PRODUCT,
                    KeyRedisConstants.TRANSLATION,
                    KeyRedisConstants.TYPE_OF_COLLECTION);

                logger.LogInformation("✅ Cache limpiado");
                logger.LogInformation("===================================================");

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                logger.LogError(ex, "❌ ERROR CRÍTICO: Rollback ejecutado - {ErrorMessage}", ex.Message);
                logger.LogInformation("===================================================");
                return Result<CourtEntity, Error>.Failure(
                    Error.CreateInstance("CourtTransactionError", 
                        $"Error durante la transacción completa del corte: {ex.Message}", 
                        System.Net.HttpStatusCode.InternalServerError));
            }
        }

        private async Task<Result<VoidResult, Error>> ValidateAndUpdateProductPricesWithinTransactionAsync(
            DataBaseContext context,
            IEnumerable<CourtDispenserTransactionData> courtDispensers,
            CancellationToken cancellationToken)
        {
            try
            {
                var priceUpdates = new List<(int ProductId, double OldPrice, double NewPrice, string ProductName)>();

                foreach (var dispenser in courtDispensers)
                {
                    // Calcular el precio del corte (precio por galón)
                    if (dispenser.GallonsDifferenceResult <= 0)
                        continue; // No hay venta, no se puede calcular precio

                    var courtPrice = dispenser.AmountDifferenceResult / dispenser.GallonsDifferenceResult;

                    // Obtener información del producto
                    var productAndCompartiment = await getProductAndCompartiment
                        .GetProductAndCompartimentAsync(dispenser.IdHose);

                    // Obtener el producto actual desde el contexto de la transacción
                    var product = await context.Product
                        .FirstOrDefaultAsync(p => p.IdProduct == productAndCompartiment.IdProduct, cancellationToken);

                    if (product == null)
                    {
                        return Error.BadRequest("ProductNotFound", 
                            $"No se encontró el producto con Id {productAndCompartiment.IdProduct}");
                    }

                    var currentSellPrice = product.SellPrice ?? 0;

                    // Validar si el precio es diferente (usando tolerancia para comparación de decimales)
                    if (Math.Abs(currentSellPrice - courtPrice) > 0.01)
                    {
                        // Actualizar el precio del producto dentro de la transacción
                        var previousPrice = product.SellPrice;
                        product.SellPrice = courtPrice;
                        context.Product.Update(product);

                        priceUpdates.Add((product.IdProduct, currentSellPrice, courtPrice, product.Name));
                    }
                }

                // Guardar cambios de precios
                if (priceUpdates.Any())
                {
                    var pricesSaveResult = await context.SaveChangesAsync(cancellationToken) > 0;
                    if (!pricesSaveResult)
                    {
                        return Error.Internal("PriceUpdateError", 
                            "Error al guardar los cambios de precios");
                    }
                }

                // Log de auditoría de precios actualizados
                if (priceUpdates.Any())
                {
                    logger.LogInformation("=== PRECIOS ACTUALIZADOS AUTOMÁTICAMENTE DESDE CORTE ===");
                    foreach (var (productId, oldPrice, newPrice, productName) in priceUpdates)
                    {
                        logger.LogInformation("💰 Producto {ProductId} ({ProductName}): ${OldPrice:F2} -> ${NewPrice:F2}", 
                            productId, productName, oldPrice, newPrice);
                    }
                    logger.LogInformation("Total productos actualizados: {UpdateCount}", priceUpdates.Count);
                    logger.LogInformation("=========================================================");
                }
                else
                {
                    logger.LogInformation("No se detectaron discrepancias de precios en el corte");
                }

                return Result<VoidResult, Error>.Success(VoidResult.Instance);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar precios de productos: {ErrorMessage}", ex.Message);
                return Error.Internal("ProductPriceUpdateError", 
                    $"Error al actualizar precios de productos: {ex.Message}");
            }
        }

        private async Task<Result<VoidResult, Error>> UpdateProductInventoryAsync(
            DataBaseContext context,
            IEnumerable<CourtDispenserSaleEntity> courtDispensers,
            CancellationToken cancellationToken)
        {
            foreach (var dispenser in courtDispensers)
            {
                // Obtener el producto
                var product = await context.Product
                    .FirstOrDefaultAsync(p => p.IdProduct == dispenser.IdProduct, cancellationToken);

                if (product == null)
                {
                    return Error.BadRequest("ProductNotFound", 
                        $"No se encontró el producto con Id {dispenser.IdProduct}");
                }

                // Validar que hay suficiente stock
                var nuevoStock = product.Stock - dispenser.GallonsDifferenceResult;
                if (nuevoStock < 0)
                {
                    return Error.BadRequest("InsufficientStock",
                        $"Stock insuficiente para el producto {dispenser.IdProduct}. " +
                        $"Stock actual: {product.Stock}, Cantidad solicitada: {dispenser.GallonsDifferenceResult}");
                }

                // Actualizar el stock
                product.Stock = nuevoStock;
                context.Product.Update(product);

                logger.LogInformation("📦 Stock actualizado para producto {ProductId}: {OldStock} -> {NewStock} (Vendidos: {SoldGallons} gal)",
                    product.IdProduct,
                    product.Stock + dispenser.GallonsDifferenceResult,
                    product.Stock,
                    dispenser.GallonsDifferenceResult);
            }

            // Guardar cambios de inventario
            var stockSaveResult = await context.SaveChangesAsync(cancellationToken) > 0;
            if (!stockSaveResult)
            {
                return Error.Internal("InventoryUpdateError", 
                    "Error al actualizar el inventario de productos");
            }

            return Result<VoidResult, Error>.Success(VoidResult.Instance);
        }
    }
}
