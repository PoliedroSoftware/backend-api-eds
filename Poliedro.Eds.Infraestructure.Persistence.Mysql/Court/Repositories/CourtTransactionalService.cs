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
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories
{
    public class CourtTransactionalService(
        ITenantDbContextFactory dbContextFactory,
        IGetProductAndCompartiment getProductAndCompartiment,
        ILogger<CourtTransactionalService> logger,
        IRedisService redisService,
        IDomainEventDispatcher domainEventDispatcher,
        IHttpContextAccessor httpContextAccessor,
        ICourtDomainService courtDomainService,
        IHoseUpdateHose hoseUpdateService,
        IHoseGetByIdHose hoseGetByIdService,
        IProductUpdateProduct productUpdateService,
        IProductGetByIdProduct productGetByIdService) : ICourtTransactionalService
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

                // 1. Crear el corte usando domain service
                var courtSaveResult = await courtDomainService.CreateAsync(courtEntity);
                if (!courtSaveResult.IsSuccess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    logger.LogError("❌ ERROR: Falló la creación del corte");
                    return Result<CourtEntity, Error>.Failure(courtSaveResult.Error!);
                }
                logger.LogInformation("✅ Corte creado exitosamente");

                // 2. Actualizar hoses con los valores acumulados del corte usando domain service
                var hoseUpdateResult = await UpdateHoseAccumulatedValuesWithDomainServiceAsync(
                    courtEntity.CourtDispensers, cancellationToken);
                
                if (!hoseUpdateResult.IsSuccess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    logger.LogError("❌ ERROR: Falló la actualización de mangueras - {ErrorDescription}", 
                        hoseUpdateResult.Error?.Description);
                    return Result<CourtEntity, Error>.Failure(hoseUpdateResult.Error!);
                }
                logger.LogInformation("✅ Mangueras actualizadas exitosamente");

                // 3. Actualizar inventario de productos (reducir stock) usando domain service
                if (courtDispenserSaleEntities.Any())
                {
                    var inventoryUpdateResult = await UpdateProductInventoryWithDomainServiceAsync(
                        courtDispenserSaleEntities, cancellationToken);
                    
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
                logger.LogInformation("✅ TRANSACCIÓN CONFIRMADA EXITOSAMENTE");

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

                // 7. Invalidar caché de mangueras afectadas
                await InvalidateHoseCacheAsync(result, courtEntity.CourtDispensers);

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

                // 1. Validar y actualizar precios de productos usando domain service
                var priceValidationResult = await ValidateAndUpdateProductPricesWithDomainServiceAsync(
                    courtDispensers, cancellationToken);

                if (!priceValidationResult.IsSuccess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    logger.LogError("❌ ERROR: Falló la validación de precios - {ErrorDescription}", 
                        priceValidationResult.Error?.Description);
                    return Result<CourtEntity, Error>.Failure(priceValidationResult.Error!);
                }
                logger.LogInformation("✅ Precios validados y actualizados");

                // 2. Crear el corte usando domain service
                var courtSaveResult = await courtDomainService.CreateAsync(courtEntity);
                if (!courtSaveResult.IsSuccess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    logger.LogError("❌ ERROR: Falló la creación del corte");
                    return Result<CourtEntity, Error>.Failure(courtSaveResult.Error!);
                }
                logger.LogInformation("✅ Corte creado exitosamente");

                // 3. Actualizar hoses con los valores acumulados del corte usando domain service
                var hoseUpdateResult = await UpdateHoseAccumulatedValuesWithDomainServiceAsync(
                    courtEntity.CourtDispensers, cancellationToken);
                
                if (!hoseUpdateResult.IsSuccess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    logger.LogError("❌ ERROR: Falló la actualización de mangueras - {ErrorDescription}", 
                        hoseUpdateResult.Error?.Description);
                    return Result<CourtEntity, Error>.Failure(hoseUpdateResult.Error!);
                }
                logger.LogInformation("✅ Mangueras actualizadas exitosamente");

                // 4. Actualizar inventario de productos (reducir stock) usando domain service
                if (courtDispenserSaleEntities.Any())
                {
                    var inventoryUpdateResult = await UpdateProductInventoryWithDomainServiceAsync(
                        courtDispenserSaleEntities, cancellationToken);
                    
                    if (!inventoryUpdateResult.IsSuccess)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        logger.LogError("❌ ERROR: Falló la actualización de inventario - {ErrorDescription}", 
                            inventoryUpdateResult.Error?.Description);
                        return Result<CourtEntity, Error>.Failure(inventoryUpdateResult.Error!);
                    }
                    logger.LogInformation("✅ Inventario actualizado exitosamente");
                }

                // 5. Confirmar la transacción
                await transaction.CommitAsync(cancellationToken);
                logger.LogInformation("✅ TRANSACCIÓN COMPLETA CONFIRMADA EXITOSAMENTE");

                var result = Result<CourtEntity, Error>.Success(courtEntity);
                
                // 6. Invalidar caché del corte usando sistema distribuido
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result, 
                    redisService, 
                    domainEventDispatcher, 
                    httpContextAccessor,
                    "court",
                    "create",
                    courtEntity.IdCourt);

                // 7. Invalidar caché del inventario y productos afectados
                await InvalidateInventoryAndProductCacheAsync(result, courtDispenserSaleEntities);

                // 8. Invalidar caché de mangueras afectadas
                await InvalidateHoseCacheAsync(result, courtEntity.CourtDispensers);

                // 8. Invalidar caché de Listas de court afectadas
                await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, KeyRedisConstants.COURT_LIST_SERVICE);

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

        /// <summary>
        /// Actualiza los valores acumulados de las mangueras con los datos del corte usando domain services
        /// </summary>
        private async Task<Result<VoidResult, Error>> UpdateHoseAccumulatedValuesWithDomainServiceAsync(
            IEnumerable<CourtDispenserEntity> courtDispensers,
            CancellationToken cancellationToken)
        {
            try
            {
                var hoseUpdates = new List<(int HoseId, double OldAmount, double NewAmount, double OldGallons, double NewGallons)>();

                foreach (var courtDispenser in courtDispensers)
                {
                    // Obtener la manguera actual usando domain service
                    var hoseResult = await hoseGetByIdService.GetByIdAsync(courtDispenser.IdHose);
                    if (!hoseResult.IsSuccess)
                    {
                        return Error.BadRequest("HoseNotFound", 
                            $"No se encontró la manguera con Id {courtDispenser.IdHose}");
                    }

                    var hose = hoseResult.Value!;
                    var oldAmount = hose.AccumulatedAmount;
                    var oldGallons = hose.AccumulatedGallons;

                    // Actualizar los valores acumulados de la manguera
                    hose.AccumulatedAmount = courtDispenser.AccumulatedAmount;
                    hose.AccumulatedGallons = courtDispenser.AccumulatedGallons;
                    
                    // Usar domain service para actualizar
                    var updateResult = await hoseUpdateService.UpdateAsync(hose);
                    if (!updateResult.IsSuccess)
                    {
                        return updateResult.Error!;
                    }

                    hoseUpdates.Add((hose.IdHose, oldAmount, courtDispenser.AccumulatedAmount, 
                        oldGallons, courtDispenser.AccumulatedGallons));
                }

                // Log de auditoría de mangueras actualizadas
                if (hoseUpdates.Any())
                {
                    logger.LogInformation("=== MANGUERAS ACTUALIZADAS DESDE CORTE ===");
                    foreach (var (hoseId, oldAmount, newAmount, oldGallons, newGallons) in hoseUpdates)
                    {
                        logger.LogInformation("🔧 Manguera {HoseId}: Monto ${OldAmount:F2} -> ${NewAmount:F2}, Galones {OldGallons:F3} -> {NewGallons:F3} gls", 
                            hoseId, oldAmount, newAmount, oldGallons, newGallons);
                    }
                    logger.LogInformation("Total mangueras actualizadas: {UpdateCount}", hoseUpdates.Count);
                    logger.LogInformation("=============================================");
                }

                return Result<VoidResult, Error>.Success(VoidResult.Instance);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar valores acumulados de mangueras: {ErrorMessage}", ex.Message);
                return Error.Internal("HoseUpdateError", 
                    $"Error al actualizar valores acumulados de mangueras: {ex.Message}");
            }
        }

        private async Task<Result<VoidResult, Error>> ValidateAndUpdateProductPricesWithDomainServiceAsync(
            IEnumerable<CourtDispenserTransactionData> courtDispensers,
            CancellationToken cancellationToken)
        {
            try
            {
                var priceUpdates = new List<(int ProductId, decimal OldPrice, decimal NewPrice, string ProductName)>();

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

                    // Obtener el producto actual usando domain service
                    var productResult = await productGetByIdService.GetByIdAsync(productAndCompartiment.IdProduct);
                    if (!productResult.IsSuccess)
                    {
                        return Error.BadRequest("ProductNotFound", 
                            $"No se encontró el producto con Id {productAndCompartiment.IdProduct}");
                    }

                    var product = productResult.Value!;
                    var currentSellPrice = product.SellPrice ?? 0;

                    // Validar si el precio es diferente (usando tolerancia para comparación de decimales)
                    if (Math.Abs(currentSellPrice - (decimal)courtPrice) > 0.01m)
                    {
                        // Actualizar el precio del producto usando domain service
                        var previousPrice = product.SellPrice;
                        product.SellPrice = (decimal)courtPrice;
                        
                        var updateResult = await productUpdateService.UpdateAsync(product);
                        if (!updateResult.IsSuccess)
                        {
                            return updateResult.Error!;
                        }

                        priceUpdates.Add((product.IdProduct, currentSellPrice, (decimal)courtPrice, product.Name));
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

        private async Task<Result<VoidResult, Error>> UpdateProductInventoryWithDomainServiceAsync(
            IEnumerable<CourtDispenserSaleEntity> courtDispensers,
            CancellationToken cancellationToken)
        {
            foreach (var dispenser in courtDispensers)
            {
                // Obtener el producto usando domain service
                var productResult = await productGetByIdService.GetByIdAsync(dispenser.IdProduct);
                if (!productResult.IsSuccess)
                {
                    return Error.BadRequest("ProductNotFound", 
                        $"No se encontró el producto con Id {dispenser.IdProduct}");
                }

                var product = productResult.Value!;

                // Validar que hay suficiente stock
                var nuevoStock = product.Stock - (decimal)dispenser.GallonsDifferenceResult;
                //if (nuevoStock < 0)
                //{
                //    return Error.BadRequest("InsufficientStock",
                //        $"Stock insuficiente para el producto {dispenser.IdProduct}. " +
                //        $"Stock actual: {product.Stock}, Cantidad solicitada: {dispenser.GallonsDifferenceResult}");
                //}

                // Actualizar el stock
                var oldStock = product.Stock;
                product.Stock = nuevoStock;
                
                // Usar domain service para actualizar
                var updateResult = await productUpdateService.UpdateAsync(product);
                if (!updateResult.IsSuccess)
                {
                    return updateResult.Error!;
                }

                logger.LogInformation("📦 Stock actualizado para producto {ProductId}: {OldStock} -> {NewStock} (Vendidos: {SoldGallons} gal)",
                    product.IdProduct,
                    oldStock,
                    product.Stock,
                    dispenser.GallonsDifferenceResult);
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

        /// <summary>
        /// Invalida la caché de las mangueras afectadas por el corte
        /// </summary>
        private async Task InvalidateHoseCacheAsync(
            Result<CourtEntity, Error> result, 
            IEnumerable<CourtDispenserEntity> courtDispensers)
        {
            if (!result.IsSuccess) return;

            try
            {
                // Invalidar cache general de mangueras usando sistema distribuido
                await RedisHelper.InvalidateDistributedCacheAsync(
                    result,
                    redisService,
                    domainEventDispatcher,
                    httpContextAccessor,
                    "hose",
                    "update",
                    null);

                // Invalidar cache específico de cada manguera afectada
                foreach (var courtDispenser in courtDispensers)
                {
                    await RedisHelper.InvalidateDistributedCacheAsync(
                        result,
                        redisService,
                        domainEventDispatcher,
                        httpContextAccessor,
                        "hose",
                        "update",
                        courtDispenser.IdHose);
                }

                // También invalidar las constantes de cache tradicionales como respaldo
                await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService, 
                    KeyRedisConstants.HOSE,
                    KeyRedisConstants.HOSE_HISTORY,
                    "LastAccumulated:");

                logger.LogInformation("🗑️ Cache de mangueras invalidado exitosamente - {HoseCount} mangueras afectadas", 
                    courtDispensers.Count());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Error al invalidar cache de mangueras: {ErrorMessage}", ex.Message);
                // No lanzamos excepción porque la transacción ya fue confirmada exitosamente
            }
        }
    }
}
