using MediatR;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetTotalBalance;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Court.DomainService;
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
    IMediator mediator
    ) : IRequestHandler<SendWhatsAppMessageCommand, Unit>
{
    public async Task<Unit> Handle(SendWhatsAppMessageCommand request, CancellationToken cancellationToken)
    {
        var court = request.Court;

        // Obtener todos los números de teléfono usando el servicio IPhoneGetAllService
        var phoneNumbers = await getPhone.GetAllAsync(new PaginationParams { PageNumber = 1, PageSize = 1000 });

        // Convertir los números de teléfono en una lista de strings
        var phoneNumbersList = phoneNumbers.Select(p => p.Number).ToList();

        // Gastos
        var ExpenseSummary = string.Join("\n", await Task.WhenAll(
        court.CourtExpenditures?.Select(async p =>
        {
            var gastosname = await getExpenditure.GetExpenditureIdAsync(p.IdExpenditures);
            return $"{gastosname}:$ {p.Amount:N2}";
        }) ?? Enumerable.Empty<Task<string>>()
         ));

        var totalExpenditures = court.CourtExpenditures?.Sum(e => e?.Amount ?? 0) ?? 0;


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

        //Mangueras y Dispensadores

        // Agrupar por DispensadorId, luego construir el mensaje agrupado con utilidades
        var hosesGrouped = await Task.WhenAll(
            court.CourtDispensers.Select(async d =>
            {
                var hoseNumber = await getHose.GetHoseNumberAsync(d.IdHose);
                var idDispenser = await getdispenserNumber.GetDispenserNumberAsync(d.IdHose);
                
                // Obtener información del producto para calcular utilidad
                var productAndCompartiment = await getProductAndCompartiment.GetProductAndCompartimentAsync(d.IdHose);
                var productResult = await getProductById.GetByIdAsync(productAndCompartiment.IdProduct);
                
                double utilityPerHose = 0;
                string productName = "Producto Desconocido";
                
                if (productResult.IsSuccess && productResult.Value != null)
                {
                    var product = productResult.Value;
                    productName = product.Name ?? "Producto Sin Nombre";
                    var sellPrice = product.SellPrice ?? 0;
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
                    ProductName = productName
                };
            })
        );

        // Agrupar por dispensador y ordenar por IdDispenser
        var dispensersGrouped = hosesGrouped
            .GroupBy(h => h.Dispenser)
            .OrderBy(g => g.Key);

        // Construir string final con utilidades
        var hoseDetailString = string.Join("\n\n", dispensersGrouped.Select(group =>
        {
            var mangueras = string.Join("\n", group
                .OrderBy(h => h.Hose)
                .Select(h =>
                    $"""

            🔧 Manguera: {h.Hose}
            🛢️ Producto: {h.ProductName}
            💵 Venta En Dinero: ${h.Amount:N2}
            📊 Venta En Galones: {h.Gallons:N2} gal
            📈 Utilidad: ${h.Utility:N2}
            """));

            return $"""

    ⛽ Dispensador: {group.Key}
    {mangueras}
    """;
        }));

        var totalGallons = court.CourtDispensers?.Sum(d => d.GallonsDifferenceResult) ?? 0;
        var totalUtility = hosesGrouped.Sum(h => h.Utility);

        // Construir el mensaje final
        var message = $"""
                📋 CORTE FINALIZADO

                👨‍💼 Islero: {isleroName}

                ⏰ Inicio Turno: {court.Starttime} {court.DateStarttime}
                ⏰ Fin Turno: {court.Endtime} {court.DateEndtime}
                
                   {hoseDetailString}
               
                ════════════════════════
                📊 RESUMEN TOTAL
                ════════════════════════

                ⛽ Total Galones Vendidos: {totalGallons:N2}
                💰 Total Ventas: ${totalVentas:N2}
                📈 Total Utilidad Del Día: ${totalUtility:N2}

                ════════════════════════
                💸 GASTOS DETALLADOS
                ════════════════════════
                {ExpenseSummary}

                💸 Total En Gastos: ${totalExpenditures:N2}

                ════════════════════════
                💳 MEDIOS DE PAGO
                ════════════════════════
                {paymentSummary}

                ════════════════════════
                💼 RESUMEN FINANCIERO
                ════════════════════════
                💰 Total A Recibir En Efectivo: ${totalARecibirEnEfectivo:N2}
                🏛️ Total En Caja Fuerte: ${nuevoSaldoStrongBox:N2}

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

