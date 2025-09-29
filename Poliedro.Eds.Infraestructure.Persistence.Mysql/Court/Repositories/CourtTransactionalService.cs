using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Events;
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
        IRedisService redisService,
        IDomainEventDispatcher domainEventDispatcher,
        IHttpContextAccessor httpContextAccessor) : ICourtTransactionalService
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

                var result = Result<CourtEntity, Error>.Success(courtEntity);
                
                // 4. Invalidar caché del corte usando sistema distribuido
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result, 
                    redisService, 
                    domainEventDispatcher, 
                    httpContextAccessor,
                    "court",
                    "create",
                    courtEntity.IdCourt);

                // 5. Invalidar caché del inventario y productos afectados
                await InvalidateInventoryAndProductCacheAsync(result, courtDispenserSaleEntities);

                logger.LogInformation("✅ Cache invalidado usando sistema distribuido");
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

                var result = Result<CourtEntity, Error>.Success(courtEntity);
                
                // 5. Invalidar caché del corte usando sistema distribuido
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result, 
                    redisService, 
                    domainEventDispatcher, 
                    httpContextAccessor,
                    "court",
                    "create",
                    courtEntity.IdCourt);

                // 6. Invalidar caché del inventario y productos afectados
                await InvalidateInventoryAndProductCacheAsync(result, courtDispenserSaleEntities);

                logger.LogInformation("✅ Cache invalidado usando sistema distribuido");
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

                    // Redondear los valores para evitar problemas de precisión de punto flotante
                    var roundedAmount = Math.Round(dispenser.AmountDifferenceResult, 2);
                    var roundedGallons = Math.Round(dispenser.GallonsDifferenceResult, 3);
                    
                    // Calcular el precio y redondearlo al entero más cercano (sin decimales)
                    var calculatedPrice = roundedAmount / roundedGallons;
                    var courtPrice = Math.Round(calculatedPrice, 0); // Redondear a 0 decimales (entero)

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
                        logger.LogInformation("💰 Producto {ProductId} ({ProductName}): ${OldPrice:F0} -> ${NewPrice:F0}", 
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

        /// <summary>
        /// Invalida la caché del inventario y productos afectados por el corte
        /// </summary>
        private async Task InvalidateInventoryAndProductCacheAsync(
            Result<CourtEntity, Error> result, 
            IEnumerable<CourtDispenserSaleEntity> courtDispenserSaleEntities)
        {
            if (!result.IsSuccess) return;

            try
            {
                // Invalidar cache del inventario usando sistema distribuido
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result,
                    redisService,
                    domainEventDispatcher,
                    httpContextAccessor,
                    "inventory",
                    "update",
                    null);

                // Invalidar cache de productos usando sistema distribuido
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result,
                    redisService,
                    domainEventDispatcher,
                    httpContextAccessor,
                    "product",
                    "update",
                    null);

                // Invalidar cache específico de cada producto afectado
                foreach (var courtDispenser in courtDispenserSaleEntities)
                {
                    await RedisHelper.InvalidateDistributedCacheAsync(
                        result,
                        redisService,
                        domainEventDispatcher,
                        httpContextAccessor,
                        "product",
                        "update",
                        courtDispenser.IdProduct);
                }

                // Invalidar cache de compartimentos ya que el stock puede afectar la información de compartimentos
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result,
                    redisService,
                    domainEventDispatcher,
                    httpContextAccessor,
                    "compartiment",
                    "update",
                    null);

                // También invalidar las constantes de cache tradicionales como respaldo
                await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, 
                    KeyRedisConstants.INVENTORY,
                    KeyRedisConstants.PRODUCT, 
                    "inventoryListService:",
                    KeyRedisConstants.COMPARTIMENT);

                logger.LogInformation("🗑️ Cache de inventario y productos invalidado exitosamente");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Error al invalidar cache de inventario y productos: {ErrorMessage}", ex.Message);
                // No lanzamos excepción porque la transacción ya fue confirmada exitosamente
            }
        }
    }
}
