using MediatR;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetTotalBalance;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Phone.DomainServices.GetAll;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.SendMessage;
using System.Globalization;

public class SendWhatsAppMessageCommandHandler(
    ISendMessage sendMessage,
    IGetPaymentMethodName getPaymentMethodName,
    IGetExpenditureName getExpenditure,
    IIslanderGetAllIslander getIsleros,
    IGetHoseNumber getHose,
    IGetDispenserNumber getdispenserNumber,
    IPhoneGetAllService getPhone,
    IGetProductAndCompartiment getProductAndCompartiment,
    IProductGetByIdProduct getProductById,
    IMediator mediator,
    IEdsGetByIdService edsGetByIdService
    ) : IRequestHandler<SendWhatsAppMessageCommand, Unit>
{
    // Cultura española para usar coma como separador decimal
    private static readonly CultureInfo SpanishCulture = new CultureInfo("es-ES");

    /// <summary>
    /// Formatea los galones mostrando decimales solo cuando es necesario (máximo 3 decimales)
    /// </summary>
    /// <param name="gallons">Valor de galones a formatear</param>
    /// <returns>String formateado con decimales dinámicos</returns>
    private static string FormatGallons(double gallons)
    {
        // Si es un número entero, mostrar sin decimales
        if (gallons == Math.Floor(gallons))
        {
            return gallons.ToString("N0", SpanishCulture);
        }
        
        // Si tiene decimales, determinar cuántos decimales significativos mostrar (máximo 3)
        var rounded = Math.Round(gallons, 3);
        
        // Convertir a string con 3 decimales y luego remover ceros al final
        var formatted = rounded.ToString("N3", SpanishCulture);
        
        // Remover ceros trailing después del separador decimal
        if (formatted.Contains(','))
        {
            formatted = formatted.TrimEnd('0').TrimEnd(',');
        }
        
        return formatted;
    }

    public async Task<Unit> Handle(SendWhatsAppMessageCommand request, CancellationToken cancellationToken)
    {
        var court = request.Court;

        // Obtener todos los números de teléfono usando el servicio IPhoneGetAllService
        var phoneNumbers = await getPhone.GetAllAsync(new PaginationParams { PageNumber = 1, PageSize = 1000 });

        // Convertir los números de teléfono en una lista de strings
        var phoneNumbersList = phoneNumbers.Select(p => p.Number).ToList();

        // Verificar si existen gastos
        var hasExpenditures = court.CourtExpenditures?.Any() == true && court.CourtExpenditures.Any(e => e != null);

        // Gastos (solo procesar si existen)
        var ExpenseSummary = string.Empty;
        var totalExpenditures = 0.0;

        if (hasExpenditures)
        {
            ExpenseSummary = string.Join("\n", await Task.WhenAll(
                court.CourtExpenditures!.Where(p => p != null).Select(async p =>
                {
                    var gastosname = await getExpenditure.GetExpenditureIdAsync(p.IdExpenditures);
                    return $"{gastosname}:$ {p.Amount.ToString("N0", SpanishCulture)}";
                })
            ));

            totalExpenditures = court.CourtExpenditures.Sum(e => e?.Amount ?? 0);
        }

        // Medios de pago
        var paymentSummary = string.Join("\n", await Task.WhenAll(
        court.CourtTypeOfCollections.Select(async p =>
        {
            var paymentMethodName = await getPaymentMethodName.GetPaymentMethodNameAsync(p.IdTypeOfCollection);
            return $"{paymentMethodName}: $ {p.Amount.ToString("N0", SpanishCulture)}";
        })
         ));

        // Sumar solo los montos con el método de pago "Efectivo"
        var sumEfectivo = court.CourtTypeOfCollections?
            .Where(p => getPaymentMethodName.GetPaymentMethodNameAsync(p.IdTypeOfCollection).Result == "Efectivo") // Filtrar por "Efectivo"
            .Sum(p => p.Amount) ?? 0;

        // Restar los gastos del efectivo para obtener el total a recibir
        var totalARecibirEnEfectivo = sumEfectivo - totalExpenditures;

        // Obtener el saldo actual del strongbox
        var strongBoxBalance = await mediator.Send(new StrongBoxGetTotalBalance(), cancellationToken);
        var saldoActualStrongBox = strongBoxBalance?.Saldo ?? 0.0;
        
        // Calcular el nuevo saldo que quedaría en el strongbox después del corte
        var nuevoSaldoStrongBox = saldoActualStrongBox + totalARecibirEnEfectivo;

        var totalVentas = court.CourtTypeOfCollections?.Sum(p => p.Amount) ?? 0;

        // Obtener todos los isleros usando GetAllAsync
        var isleros = await getIsleros.GetAllAsync(new PaginationParams { PageNumber = 1, PageSize = 1000 });

        // Buscar el islero con el idIslander
        var islero = isleros.FirstOrDefault(i => i.IdIslander == court.IdIslander);
        var isleroName = islero?.Name ?? "Desconocido";

        // Obtener el nombre de la EDS
        var edsResult = await edsGetByIdService.GetByIdAsync(court.IdEds);
        var edsName = edsResult.IsSuccess && edsResult.Value != null 
            ? edsResult.Value.Name 
            : "EDS";

        //Mangueras y Dispensadores

        // Agrupar por DispensadorId, luego construir el mensaje agrupado con utilidades y stock
        var hosesGrouped = await Task.WhenAll(
            court.CourtDispensers.Select(async d =>
            {
                var hoseNumber = await getHose.GetHoseNumberAsync(d.IdHose);
                var idDispenser = await getdispenserNumber.GetDispenserNumberAsync(d.IdHose);
                
                // Obtener información del producto para calcular utilidad y stock
                var productAndCompartiment = await getProductAndCompartiment.GetProductAndCompartimentAsync(d.IdHose);
                var productResult = await getProductById.GetByIdAsync(productAndCompartiment.IdProduct);
                
                double utilityPerHose = 0;
                double sellPrice = 0;
                double stock = 0;
                string productName = "Producto Desconocido";
                int productId = productAndCompartiment.IdProduct;
                
                if (productResult.IsSuccess && productResult.Value != null)
                {
                    var product = productResult.Value;
                    productName = product.Name ?? "Producto Sin Nombre";
                    sellPrice = product.SellPrice ?? 0;
                    stock = product.Stock ?? 0;
                    var purchasePrice = product.PurchasePrice ?? 0;
                    var utilityPerGallon = sellPrice - purchasePrice;
                    utilityPerHose = utilityPerGallon * d.GallonsDifferenceResult;
                }
                
                return new
                {
                    Dispenser = idDispenser,
                    Hose = hoseNumber,
                    Amount = d.AmountDifferenceResult,
                    Gallons = d.GallonsDifferenceResult,
                    Utility = utilityPerHose,
                    ProductName = productName,
                    SellPrice = sellPrice,
                    Stock = stock,
                    ProductId = productId
                };
            })
        );

        // Agrupar por dispensador y ordenar por IdDispenser
        var dispensersGrouped = hosesGrouped
            .GroupBy(h => h.Dispenser)
            .OrderBy(g => g.Key);

        // Construir string final con utilidades, precio por galón y stock
        var hoseDetailString = string.Join("\n\n", dispensersGrouped.Select(group =>
        {
            var mangueras = string.Join("\n", group
                .OrderBy(h => h.Hose)
                .Select(h =>
                    $"""

            🔧 Manguera: {h.Hose}
            🛢️ Producto: {h.ProductName}
            💵 Venta En Dinero: ${h.Amount.ToString("N0", SpanishCulture)}
            📊 Venta En Galones: {FormatGallons(h.Gallons)} gl
            💰 Precio por Galón: ${h.SellPrice.ToString("N0", SpanishCulture)}
            📈 Utilidad: ${h.Utility.ToString("N0", SpanishCulture)}
            📦 Stock Actual: {FormatGallons(h.Stock)} gl
            """));

            return $"""

    ⛽ Dispensador: {group.Key}
    {mangueras}
    """;
        }));

        var totalGallons = court.CourtDispensers?.Sum(d => d.GallonsDifferenceResult) ?? 0;
        var totalUtility = hosesGrouped.Sum(h => h.Utility);


        // Construir la sección de gastos condicionalmente
        var gastosSection = hasExpenditures ? $"""

                ══════════════
                💸 GASTOS DETALLADOS
                ══════════════
                {ExpenseSummary}

                💸 Total En Gastos: ${totalExpenditures.ToString("N0", SpanishCulture)}
                """ : string.Empty;

        // Construir la sección de observaciones condicionalmente
        var observacionesSection = !string.IsNullOrWhiteSpace(court.Descripcion) ? $"""

                ══════════════
                📝 OBSERVACIONES
                ══════════════
                {court.Descripcion}
                """ : string.Empty;

        // Construir el mensaje final
        var message = $"""
                📋 CORTE {edsName} FINALIZADO

                👨‍💼 Islero: {isleroName}

                ⏰ Inicio Turno: {court.Starttime} {court.DateStarttime}
                ⏰ Fin Turno: {court.Endtime} {court.DateEndtime}
                
                   {hoseDetailString}
               
                ══════════════
                📊 RESUMEN TOTAL
                ══════════════

                ⛽ Total Galones Vendidos: {FormatGallons(totalGallons)} gl
                💰 Total Ventas: ${totalVentas.ToString("N0", SpanishCulture)}
                📈 Total Utilidad Del Día: ${totalUtility.ToString("N0", SpanishCulture)}
                {gastosSection}
               
                ══════════════
                💳 MEDIOS DE PAGO
                ══════════════
                {paymentSummary}

                ══════════════
                💼 RESUMEN FINANCIERO
                ══════════════
                💰 Total A Recibir En Efectivo: ${totalARecibirEnEfectivo.ToString("N0", SpanishCulture)}
                🏛️ Total En Caja Fuerte: ${nuevoSaldoStrongBox.ToString("N0", SpanishCulture)}
                {observacionesSection}

                📎 Documentos Cargados: {court.CourtDocuments?.Count() ?? 0}
                """;

        // Enviar mensaje a cada número de teléfono
        foreach (var phoneNumber in phoneNumbersList)
        {
            await sendMessage.SendMessageAsync(phoneNumber, message);
        }

        return Unit.Value;
    }
}

