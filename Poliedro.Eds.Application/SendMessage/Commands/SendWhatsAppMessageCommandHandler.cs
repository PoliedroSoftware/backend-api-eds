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
                    return $"{gastosname}:$ {p.Amount:N2}";
                })
            ));

            totalExpenditures = court.CourtExpenditures.Sum(e => e?.Amount ?? 0);
        }

        // Medios de pago
        var paymentSummary = string.Join("\n", await Task.WhenAll(
        court.CourtTypeOfCollections.Select(async p =>
        {
            var paymentMethodName = await getPaymentMethodName.GetPaymentMethodNameAsync(p.IdTypeOfCollection);
            return $"{paymentMethodName}: $ {p.Amount:N2}";
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
            💵 Venta En Dinero: ${h.Amount:N0}
            📊 Venta En Galones: {h.Gallons:N0} gl
            💰 Precio por Galón: ${h.SellPrice:N0}
            📈 Utilidad: ${h.Utility:N0}
            📦 Stock Actual: {h.Stock:N0} gl
            """));

            return $"""

    ⛽ Dispensador: {group.Key}
    {mangueras}
    """;
        }));

        var totalGallons = court.CourtDispensers?.Sum(d => d.GallonsDifferenceResult) ?? 0;
        var totalUtility = hosesGrouped.Sum(h => h.Utility);

        // Crear resumen de stock por producto (agrupando productos únicos)
        var stockSummary = hosesGrouped
            .GroupBy(h => new { h.ProductId, h.ProductName })
            .Select(g => new
            {
                ProductName = g.Key.ProductName,
                Stock = g.First().Stock // Todos los elementos del grupo tienen el mismo stock
            })
            .OrderBy(p => p.ProductName)
            .Select(p => $"🛢️ {p.ProductName}: {p.Stock:N0} gl")
            .ToList();

        var stockSection = stockSummary.Any() ? $"""

                ══════════════
                📦 INVENTARIO ACTUAL
                ══════════════
                {string.Join("\n", stockSummary)}
                """ : string.Empty;

        // Construir la sección de gastos condicionalmente
        var gastosSection = hasExpenditures ? $"""

                ══════════════
                💸 GASTOS DETALLADOS
                ══════════════
                {ExpenseSummary}

                💸 Total En Gastos: ${totalExpenditures:N0}
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

                ⛽ Total Galones Vendidos: {totalGallons:N0} gl
                💰 Total Ventas: ${totalVentas:N0}
                📈 Total Utilidad Del Día: ${totalUtility:N0}
                {gastosSection}
                {stockSection}

                ══════════════
                💳 MEDIOS DE PAGO
                ══════════════
                {paymentSummary}

                ══════════════
                💼 RESUMEN FINANCIERO
                ══════════════
                💰 Total A Recibir En Efectivo: ${totalARecibirEnEfectivo:N0}
                🏛️ Total En Caja Fuerte: ${nuevoSaldoStrongBox:N0}

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

