using System.Globalization;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.TransferValidation.Dtos;
using Poliedro.Eds.Application.TransferValidation.Validation;
using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Phone.DomainServices.GetAll;
using Poliedro.Eds.Domain.SendMessage;
using Poliedro.Eds.Domain.TransferValidation.Exceptions;
using Poliedro.Eds.Domain.TransferValidation.Services;

namespace Poliedro.Eds.Application.TransferValidation.Commands.CreateTransferValidation;

public class CreateTransferValidationCommandHandler(
    ITransferValidationService transferValidationService,
    IMapper mapper,
    TransferValidationCreateValidator validator,
    ILogger<CreateTransferValidationCommandHandler> logger,
    ISendMessage sendMessage,
    IPhoneGetAllService phoneGetAllService)
    : IRequestHandler<CreateTransferValidationCommand, TransferValidationDto>
{
    
    private static readonly CultureInfo SpanishCulture = new CultureInfo("es-ES");

    public async Task<TransferValidationDto> Handle(
        CreateTransferValidationCommand request,
        CancellationToken cancellationToken)
    {
        var adjustedAmount = request.Request.TransactionAmount / 100;

        logger.LogInformation("=== INICIANDO CREACIÓN DE VALIDACIÓN DE TRANSFERENCIA ===");
        logger.LogInformation("UniqueId: {UniqueId}, Cliente: {CustomerName}, Monto: ${Amount:F2}",
        request.Request.UniqueId,
            request.Request.CustomerName,
            adjustedAmount);

        
        await validator.ValidateAndThrowAsync(request.Request, cancellationToken);

        try
        {
           
            var created = await transferValidationService.CreateAsync(
                  uniqueId: request.Request.UniqueId,
                  customerName: request.Request.CustomerName,
                  transactionAmount: adjustedAmount,
                  transactionDate: request.Request.TransactionDate,
                  transactionTime: request.Request.TransactionTime,
                  status: request.Request.Status,
                  confirmedBy: request.Request.ConfirmedBy,
                  cancellationToken: cancellationToken);

            logger.LogInformation("✅ Validación de transferencia creada exitosamente con ID: {Id}",
                 created.IdTransferValidation);

            
            try
            {
                await SendWhatsAppNotificationAsync(created, cancellationToken);
                logger.LogInformation("📱 Mensajes de WhatsApp enviados exitosamente");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "⚠️ Error al enviar notificación de WhatsApp, pero la validación se creó correctamente");
            }

            logger.LogInformation("=========================================================");
            return mapper.Map<TransferValidationDto>(created);
        }
        catch (TransferValidationDomainException ex)
        {
            logger.LogError(ex, "❌ ERROR DE DOMINIO: {ErrorMessage}", ex.Message);
            logger.LogInformation("=========================================================");

            throw new ValidationException(new[]
               {
                 new ValidationFailure("TransferValidationDomain", ex.Message)
                        });
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "❌ ERROR INESPERADO: {ErrorMessage}", ex.Message);
                        logger.LogInformation("=========================================================");
                        throw;
                    }
            }

    private async Task SendWhatsAppNotificationAsync(
          Domain.TransferValidation.Entities.TransferValidationEntity transferValidation,
          CancellationToken cancellationToken)
    {
        var phoneNumbers = await phoneGetAllService.GetAllAsync(
        new PaginationParams { PageNumber = 1, PageSize = 1000 });
        var phoneNumbersList = phoneNumbers.Select(p => p.Number).ToList();
        if (!phoneNumbersList.Any())
        {
            logger.LogWarning("No hay números de teléfono registrados para enviar la notificación");
            return;
        }

        
        var message = $@"🔔 NUEVA VALIDACIÓN DE TRANSFERENCIA
            📋 ID Único: {transferValidation.UniqueId}
            👤 Cliente: {transferValidation.CustomerName}
            💵 Monto: ${transferValidation.TransactionAmount.ToString("N0", SpanishCulture)}
            📅 Fecha: {transferValidation.TransactionDate.ToString("d/M/yyyy", SpanishCulture)}
            🕐 Hora: {transferValidation.TransactionTime.ToString("h:mm tt", SpanishCulture).ToLower()}
            📊 Estado: {transferValidation.Status}";

        if (!string.IsNullOrWhiteSpace(transferValidation.ConfirmedBy))
        {
            message += $"\n✅ Confirmado por: {transferValidation.ConfirmedBy}";
        }

       
        foreach (var phoneNumber in phoneNumbersList)
        {
            try
            {
                await sendMessage.SendMessageAsync(phoneNumber, message);
                logger.LogInformation("📱 Mensaje enviado a: {PhoneNumber}", phoneNumber);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Error al enviar mensaje a {PhoneNumber}", phoneNumber);
            }
        }
    }
}
