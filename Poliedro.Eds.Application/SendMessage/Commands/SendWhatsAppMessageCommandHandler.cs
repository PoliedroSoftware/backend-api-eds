using MediatR;
using Microsoft.AspNetCore.Http;
using Poliedro.Eds.Application.Bank.Commands;
using Poliedro.Eds.Application.Bank.Dtos;
using Poliedro.Eds.Application.Bank.Querys.BankGetCurrentBalance;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetTotalBalance;
using Poliedro.Eds.Domain.Account.Services;
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
    IEdsGetByIdService edsGetByIdService,
    IHttpContextAccessor httpContextAccessor,
    IAccountGetAllService accountGetAllService
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
        // Obtener el usuario de sesión
        var currentUser = httpContextAccessor.HttpContext?.Items["identifiername"]?.ToString() ?? "Sistema";
        
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
                    var description = !string.IsNullOrWhiteSpace(p.Description) ? $" ({p.Description})" : "";
                    return $"{gastosname}: $ {p.Amount.ToString("N0", SpanishCulture)}{description}";
                })
            ));

            totalExpenditures = court.CourtExpenditures.Sum(e => e?.Amount ?? 0);
        }

        // Medios de pago
        var paymentSummary = string.Join("\n", await Task.WhenAll(
        court.CourtTypeOfCollections.Select(async p =>
        {
            var paymentMethodName = await getPaymentMethodName.GetPaymentMethodNameAsync(p.IdTypeOfCollection);
            var description = !string.IsNullOrWhiteSpace(p.Description) ? $" ({p.Description})" : "";
            return $"{paymentMethodName}: $ {p.Amount.ToString("N0", SpanishCulture)}{description}";
        })
         ));

        // Sumar solo los montos con el método de pago "Efectivo"
        var sumEfectivo = court.CourtTypeOfCollections?
            .Where(p => getPaymentMethodName.GetPaymentMethodNameAsync(p.IdTypeOfCollection).Result == "Efectivo") // Filtrar por "Efectivo"
            .Sum(p => p.Amount) ?? 0;

        // Restar los gastos del efectivo para obtener el total a recibir
        var totalARecibirEnEfectivo = sumEfectivo - totalExpenditures;

        // Obtener el saldo currente del strongbox
        var strongBoxBalance = await mediator.Send(new StrongBoxGetTotalBalance(), cancellationToken);
        var saldoActualStrongBox = strongBoxBalance?.Saldo ?? 0.0;
        
        // Calcular el nuevo saldo que quedaría en el strongbox después del corte
        var nuevoSaldoStrongBox = saldoActualStrongBox + totalARecibirEnEfectivo;

        // === NUEVA FUNCIONALIDAD DE BANCO ===
        // Obtener medios de pago bancarios
        var bancaryPaymentMethods = new[] { "Datafono", "Nequi", "Cod_QR", "Transferencia", "Bre-B" };
        
        var bancaryPayments = court.CourtTypeOfCollections?
            .Where(p => bancaryPaymentMethods.Contains(getPaymentMethodName.GetPaymentMethodNameAsync(p.IdTypeOfCollection).Result, StringComparer.OrdinalIgnoreCase))
            .ToList() ?? new List<CourtTypeOfCollectionDto>();

        // Si hay medios de pago bancarios, crear registro en banco (no-bloqueante)
        var totalBancaryAmount = 0.0;
        if (bancaryPayments.Any())
        {
            totalBancaryAmount = bancaryPayments.Sum(p => p.Amount);
            
            // Ejecutar creación de registro bancario de forma asíncrona sin bloquear
            _ = Task.Run(async () =>
            {
                try
                {
                    // Obtener todas las cuentas bancarias
                    var accounts = await accountGetAllService.GetAllAsync();
                    
                    // Por defecto usar la primera cuenta, o se puede implementar lógica para seleccionar cuenta específica
                    var defaultAccount = accounts.FirstOrDefault();
                    
                    if (defaultAccount != null)
                    {
                        // Crear nota con detalles de los medios de pago bancarios e información de la cuenta
                        var bancaryDetails = string.Join(", ", bancaryPayments.Select(p => 
                        {
                            var paymentName = getPaymentMethodName.GetPaymentMethodNameAsync(p.IdTypeOfCollection).Result;
                            var description = !string.IsNullOrWhiteSpace(p.Description) ? $" ({p.Description})" : "";
                            return $"{paymentName}: ${p.Amount.ToString("N0", SpanishCulture)}{description}";
                        }));

                        var bancaryNote = $"Corte #{court.IdCourt} - Banco: {defaultAccount.Bank} - Cuenta: {defaultAccount.Account} - Medios de pago bancarios: {bancaryDetails}";

                        // Crear registro en banco
                        await mediator.Send(
                            new BankCreateCommand(new BankDtoCreateRequest
                            {
                                IdAccount = defaultAccount.IdAccount,
                                IdEds = court.IdEds,
                                IdCourt = court.IdCourt,
                                Moviment = "CORTE",
                                Ammount = totalBancaryAmount,
                                Note = bancaryNote
                            }),
                            cancellationToken
                        );
                    }
                }
                catch (Exception ex)
                {
                    // Log el error pero no afectar el flujo principal
                    // El logging se puede agregar aquí si hay un logger disponible
                    // Por ahora simplemente continúa sin fallar
                }
            });
        }

        var totalVentas = court.CourtTypeOfCollections?.Sum(p => p.Amount) ?? 0;

        // Obtener todos los isleros usando GetAllAsync
        var isleros = await getIsleros.GetAllAsync(new PaginationParams { PageNumber = 1, PageSize = 1000 });

        // Buscar el islero con el idIslander
        var islero = isleros.FirstOrDefault(i => i.IdIslander == court.IdIslander);
        var isleroName = islero?.Name ?? "Desconocido";

        // Obtener el nombre de la EDS
        var edsResult = await edsGetByIdService.GetByIdAsync(court.IdEds);
        var edsName = edsResult.IsSuccess && edsResult.Value != null 
            ? $"*{edsResult.Value.Name.ToUpper()}*"
            : "*EDS*";

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
                {
                    // Agregar nota informativa si el stock es negativo
                    var stockNote = h.Stock < 0 ? "\n (falta agregar la compra de este producto)" : "";
                    
                    return $"""

            🔧 Manguera: {h.Hose}
            🛢️ Producto: {h.ProductName}
            💵 Venta En Dinero: ${h.Amount.ToString("N0", SpanishCulture)}
            📊 Venta En Galones: {FormatGallons(h.Gallons)} gl
            💰 Precio por Galón: ${h.SellPrice.ToString("N0", SpanishCulture)}
            📈 Utilidad: ${h.Utility.ToString("N0", SpanishCulture)}
            📦 Stock Actual: {FormatGallons(h.Stock)} gl{stockNote}
            """;
                }));

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

        // === NUEVA SECCIÓN DE BANCO ===
        var bancarySection = string.Empty;
        string bankName = string.Empty;
        string accountNumber = string.Empty;
        double currentBankBalance = 0.0;
        
        if (bancaryPayments.Any())
        {
            // Obtener información de la cuenta bancaria para mostrar en el resumen
            try
            {
                var accounts = await accountGetAllService.GetAllAsync();
                var defaultAccount = accounts.FirstOrDefault();
                
                if (defaultAccount != null)
                {
                    bankName = defaultAccount.Bank;
                    accountNumber = defaultAccount.Account;
                    
                    // Obtener el balance actual de la cuenta bancaria
                    var currentBalance = await mediator.Send(new BankGetCurrentBalance(defaultAccount.IdAccount), cancellationToken);
                    currentBankBalance = currentBalance;
                }
            }
            catch
            {
                // Si hay error obteniendo la cuenta, usar valores por defecto
                bankName = "Banco no disponible";
                accountNumber = "Cuenta no disponible";
                currentBankBalance = totalBancaryAmount; // Solo el monto actual si no se puede obtener el balance
            }
            
            bancarySection = $"""

                ══════════════
                🏦 RESUMEN BANCARIO
                ══════════════
                🏛️ Banco: {bankName}
                📋 Cuenta: {accountNumber}
                💳 Total En Banco: ${currentBankBalance.ToString("N0", SpanishCulture)}
                """;
        }

        // Construir el mensaje final con el usuario generador
        var message = @$"📋 CORTE {edsName}

👨‍💼 Islero: {isleroName}

⏰ Inicio Turno
Hora: {court.Starttime.ToString("h:mm tt", SpanishCulture).ToLower()}
Fecha: {court.DateStarttime.ToString("d/M/yyyy", SpanishCulture)}
⏰ Fin Turno
Hora: {court.Endtime.ToString("h:mm tt", SpanishCulture).ToLower()}
Fecha: {court.DateEndtime.ToString("d/M/yyyy", SpanishCulture)}

{hoseDetailString}


══════════════
💳 MEDIOS DE PAGO
══════════════
{paymentSummary}

{gastosSection}

══════════════
📊 RESUMEN TOTAL
══════════════

⛽ Total Galones Vendidos: {FormatGallons(totalGallons)} gl
💰 Total Ventas: ${totalVentas.ToString("N0", SpanishCulture)}
📈 Total Utilidad Del Día: ${totalUtility.ToString("N0", SpanishCulture)}


══════════════
💼 RESUMEN FINANCIERO
══════════════
💰 Total A Recibir En Efectivo: ${totalARecibirEnEfectivo.ToString("N0", SpanishCulture)}
🏛️ Total En Caja Fuerte: ${nuevoSaldoStrongBox.ToString("N0", SpanishCulture)}
{bancarySection}
{observacionesSection}

📎 Documentos Cargados: {court.CourtDocuments?.Count() ?? 0}

👤 Generado por: {currentUser}";

        // Enviar mensaje a cada número de teléfono
        foreach (var phoneNumber in phoneNumbersList)
        {
            await sendMessage.SendMessageAsync(phoneNumber, message);
        }

        return Unit.Value;
    }
}

