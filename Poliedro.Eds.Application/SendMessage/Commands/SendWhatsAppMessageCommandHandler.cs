using MediatR;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Hose.DomainHose;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.SendMessage;

public class SendWhatsAppMessageCommandHandler(
    ISendMessage sendMessage,
    IGetPaymentMethodName getPaymentMethodName,
    IGetExpenditureName getExpenditure,
    IIslanderGetAllIslander getIsleros,
    IGetHoseNumber getHose,
    IGetDispenserNumber getdispenserNumber
    ) : IRequestHandler<SendWhatsAppMessageCommand, Unit>
{
    public async Task<Unit> Handle(SendWhatsAppMessageCommand request, CancellationToken cancellationToken)
    {
        var court = request.Court;


        var totalGallons = court.CourtDispensers?.Sum(d => d.AccumulatedGallons) ?? 0;


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

        var totalpagos = court.CourtTypeOfCollections?.Sum(p => p.Amount) ?? 0;

        // Obtener todos los isleros usando GetAllAsync
        var isleros = await getIsleros.GetAllAsync(new PaginationParams { PageNumber = 1, PageSize = 1000 });

        // Buscar el islero con el idIslander
        var islero = isleros.FirstOrDefault(i => i.IdIslander == court.IdIslander);
        var isleroName = islero?.Name ?? "Desconocido";

        //Mangueras y Dispensadores

        // Agrupar por DispensadorId, luego construir el mensaje agrupado
        var hosesGrouped = await Task.WhenAll(
            court.CourtDispensers.Select(async d =>
            {
                var hoseNumber = await getHose.GetHoseNumberAsync(d.IdHose);
                var idDispenser = await getdispenserNumber.GetDispenserNumberAsync(d.IdHose);
                return new
                {
                    Dispenser = idDispenser,
                    Hose = hoseNumber,
                    Amount = d.AmountDifferenceResult,
                    Gallons = d.GallonsDifferenceResult
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

            🧯 Manguera: {h.Hose}
            💵 Venta En Dinero: ${h.Amount:N2}
                Venta En Galones: {h.Gallons:N2} gal
            """));

            return $"""

    ⛽ Dispensador: {group.Key}
    {mangueras}
    """;
        }));



        var message = $"""
                📋 Corte #{court.Consecutive} finalizado

                🧑‍🔧 Islero: {isleroName}

                🕐 Inicio Turno:  {court.Starttime} {court.DateStarttime}
                🕐 Fin Turno: {court.Endtime} {court.DateEndtime} 

                
                  {hoseDetailString}
                   

                
                ⛽ Total Galones Vendidos: {totalGallons:N2}
                💰 Total Dinero Recibido: ${totalpagos:N2}
                gastos desc:
                {ExpenseSummary}

                💸 Gastos: ${totalExpenditures:N2}

                💳 Medios de pago:
                {paymentSummary}

                💰 Total Paggos: ${totalpagos:N2}

                📎 Documentos cargados: {court.CourtDocuments?.Count() ?? 0}
                🛢️ Compartimentos: 
                """;

        await sendMessage.SendMessageAsync(request.PhoneNumber, message);

        return Unit.Value;
    }
}

