using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;

namespace Poliedro.Eds.Application.Court.Services
{
    /// <summary>
    /// Service that validates product prices based on court dispenser sales data
    /// Price updates are now handled within the infrastructure transaction service
    /// </summary>
    public class ProductPriceValidationService(
        IGetProductAndCompartiment getProductAndCompartiment,
        ILogger<ProductPriceValidationService> logger) : IProductPriceValidationService
    {
        public async Task<Result<VoidResult, Error>> ValidateAndUpdateProductPricesAsync(
            IEnumerable<CourtDispenserCommand> courtDispensers,
            CancellationToken cancellationToken = default)
        {
            // Esta implementación ahora solo valida los precios
            // La actualización real se hace dentro de la transacción del CourtTransactionalService
            try
            {
                var priceCalculations = new List<(int ProductId, double CalculatedPrice, int IdHose)>();

                foreach (var dispenser in courtDispensers)
                {
                    // Calcular el precio del corte (precio por galón)
                    if (dispenser.GallonsDifferenceResult <= 0)
                        continue; // No hay venta, no se puede calcular precio

                    var courtPrice = dispenser.AmountDifferenceResult / dispenser.GallonsDifferenceResult;

                    // Obtener información del producto
                    var productAndCompartiment = await getProductAndCompartiment
                        .GetProductAndCompartimentAsync(dispenser.IdHose);

                    priceCalculations.Add((productAndCompartiment.IdProduct, courtPrice, dispenser.IdHose));
                }

                // Log de auditoría de precios calculados
                if (priceCalculations.Any())
                {
                    logger.LogInformation("=== PRECIOS CALCULADOS EN EL CORTE ===");
                    foreach (var (productId, calculatedPrice, idHose) in priceCalculations)
                    {
                        logger.LogInformation("Manguera {IdHose} - Producto {ProductId}: ${CalculatedPrice:F2}/gal",
                            idHose, productId, calculatedPrice);
                    }
                    logger.LogInformation("=========================================");
                }

                return Result<VoidResult, Error>.Success(VoidResult.Instance);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al validar precios de productos: {ErrorMessage}", ex.Message);
                return Error.Internal("ProductPriceValidationError",
                    $"Error al validar precios de productos: {ex.Message}");
            }
        }
    }
}
