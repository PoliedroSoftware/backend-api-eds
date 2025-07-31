using MediatR;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.SendMessage;

public class SendWhatsAppMessageCommandHandler(
    ISendMessage sendMessage,
    IGetPaymentMethodName getPaymentMethodName) : IRequestHandler<SendWhatsAppMessageCommand, Unit>

{

    public async Task<Unit> Handle(SendWhatsAppMessageCommand request, CancellationToken cancellationToken)
    {
        var court = request.Court;

        var totalGallons = court.CourtDispensers?.Sum(d => d.AccumulatedGallons) ?? 0; 
        var totalAmount = court.CourtDispensers?.Sum(d => d.AccumulatedAmount) ?? 0;
        var totalExpenditures = court.CourtExpenditures?.Sum(e => e?.Amount ?? 0) ?? 0;

       
        var paymentSummary = string.Join("\n", court.CourtTypeOfCollections.Select(async p =>
            $"- { await getPaymentMethodName.GetPaymentMethodNameAsync(p.IdTypeOfCollection)} : ${p.Amount:N2}"
        ));

        var message = $"""
        📋 Corte #{court.Consecutive} finalizado

        🏪 Dispensadores: {string.Empty}
            mangueras: {string.Join(", ", court.CourtDispensers.Select(d => $"#{d.IdHose}"))}

        🕐 Desde: {court.DateStarttime} {court.Starttime}
        🕐 Hasta: {court.DateEndtime} {court.Endtime}
        🧑‍🔧 Islero: #{court.IdIslander}

        ⛽ Galones vendidos: {totalGallons:N2}
        💰 Total recibido: ${totalAmount:N2}
        💸 Gastos: ${totalExpenditures:N2}

        💳 Medios de pago:
        {paymentSummary}

        📎 Documentos cargados: {court.CourtDocuments?.Count() ?? 0}
        🛢️ Compartimentos: 
        """;

        await sendMessage.SendMessageAsync(request.PhoneNumber, message);

        return Unit.Value;
    }
}

