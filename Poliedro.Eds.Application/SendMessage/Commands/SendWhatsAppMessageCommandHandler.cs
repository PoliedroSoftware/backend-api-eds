using MediatR;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.Phone.DomainServices.GetAll;
using Poliedro.Eds.Domain.SendMessage;
using Poliedro.Eds.Domain.ProductType.DomainServices;
using Poliedro.Eds.Domain.Product.DomainServices;
using Poliedro.Eds.Domain.Eds.DomainEds;

public class SendWhatsAppMessageCommandHandler(
    ISendMessage sendMessage,
    IGetPaymentMethodName getPaymentMethodName,
    IGetExpenditureName getExpenditure,
    IIslanderGetAllIslander getIsleros,
    IGetHoseNumber getHose,
    IGetDispenserNumber getdispenserNumber,
    IGetProductTypeName getProductName,
    IPhoneGetAllService getPhone,
    IGetProductCostPrice getProductCostPrice,
    IGetEdsName getEdsName
    ) : IRequestHandler<SendWhatsAppMessageCommand, Unit>
{
    public async Task<Unit> Handle(SendWhatsAppMessageCommand request, CancellationToken cancellationToken)
    {
        var court = request.Court;

        // Obtener el nombre de la EDS
        var edsName = await getEdsName.GetEdsNameAsync(court.IdEds);

        // Obtener todos los números de teléfono
        var phoneNumbers = await getPhone.GetAllAsync(new PaginationParams { PageNumber = 1, PageSize = 1000 });

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
            .Where(p => string.Equals(getPaymentMethodName.GetPaymentMethodNameAsync(p.IdTypeOfCollection).Result,"Efectivo",StringComparison.OrdinalIgnoreCase))
            .Sum(p => p.Amount- totalExpenditures) ?? 0;

        var totalVentas = court.CourtTypeOfCollections?.Sum(p => p.Amount) ?? 0;

        // Obtener todos los isleros usando GetAllAsync
        var isleros = await getIsleros.GetAllAsync(new PaginationParams { PageNumber = 1, PageSize = 1000 });

        // Buscar el islero con el idIslander
        var islero = isleros.FirstOrDefault(i => i.IdIslander == court.IdIslander);
        var isleroName = islero?.Name ?? "Desconocido";

        //Mangueras y Dispensadores
        var hosesGrouped = await Task.WhenAll(
            court.CourtDispensers.Select(async d =>
            {
                var hoseNumber = await getHose.GetHoseNumberAsync(d.IdHose);
                var idDispenser = await getdispenserNumber.GetDispenserNumberAsync(d.IdHose);
                var productName = await getProductName.GetProductTypeNameAsync(d.IdProduct);
                var costPrice = await getProductCostPrice.GetProductCostPriceAsync(d.IdProduct) ?? 0;
                var salesPrice = d.AmountDifferenceResult / d.GallonsDifferenceResult; // Precio de venta por galón
                var profitPerGallon = salesPrice - costPrice;
                var totalProfit = profitPerGallon * d.GallonsDifferenceResult;

                return new
                {
                    Dispenser = idDispenser,
                    Hose = hoseNumber,
                    Amount = d.AmountDifferenceResult,
                    Gallons = d.GallonsDifferenceResult,
                    ProductName = productName,
                    ProfitPerGallon = profitPerGallon,
                    TotalProfit = totalProfit,
                    CostPrice = costPrice,
                    SalesPrice = salesPrice
                };
            })
        );

        // Agrupar por dispensador y ordenar por IdDispenser
        var dispensersGrouped = hosesGrouped
            .GroupBy(h => h.Dispenser)
            .OrderBy(g => g.Key);

        // Construir string final
        var hoseDetailString = string.Join("\n\n", dispensersGrouped.Select(group =>
        {
            var mangueras = string.Join("\n", group
                .OrderBy(h => h.Hose)
                .Select(h =>
                    $"""

            🧯 Manguera {h.Hose} - {h.ProductName}
            💵 Venta En Dinero: ${h.Amount:N2}
                Venta En Galones: {h.Gallons:N2} gal
                Precio Costo: ${h.CostPrice:N2}/gal
                Precio Venta: ${h.SalesPrice:N2}/gal
                Utilidad por Galón: ${h.ProfitPerGallon:N2}
                Utilidad Total: ${h.TotalProfit:N2}
            """));

            return $"""

    ⛽ Dispensador: {group.Key}
    {mangueras}
    """;
        }));

        var totalProfit = hosesGrouped.Sum(h => h.TotalProfit);
        var totalGallons = court.CourtDispensers?.Sum(d => d.GallonsDifferenceResult) ?? 0;

        // Construir el mensaje final
        var message = $"""
                📋 Corte {edsName} Finalizado

                🧑‍🔧 Islero: {isleroName}

                🕐 Inicio Turno:  {court.Starttime} {court.DateStarttime}
                🕐 Fin Turno: {court.Endtime} {court.DateEndtime}
                
                   {hoseDetailString}
                   

                ⛽ Total Galones Vendidos: {totalGallons:N2}
                💰 Total Ventas: ${totalVentas:N2}
                💎 Utilidad Total: ${totalProfit:N2}

                Gastos Detallados:
                {ExpenseSummary}

                💸 Total En Gastos: ${totalExpenditures:N2}

                💳 Medios de pago:
                {paymentSummary}

                💰 Total A Recibir En Efectivo: ${sumEfectivo:N2}

                📎 Documentos cargados: {court.CourtDocuments?.Count() ?? 0}
                """;

        // Enviar mensaje a cada número de teléfono
        foreach (var phoneNumber in phoneNumbersList)
        {
            await sendMessage.SendMessageAsync(phoneNumber, message);
        }

        return Unit.Value;
    }
}

