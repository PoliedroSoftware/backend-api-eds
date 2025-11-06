using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.TransferValidation.Dtos;
using Poliedro.Eds.Domain.TransferValidation.Exceptions;
using Poliedro.Eds.Domain.TransferValidation.Services;

namespace Poliedro.Eds.Application.TransferValidation.Commands.UpdateTransferValidation;

public class UpdateTransferValidationCommandHandler(
    ITransferValidationService transferValidationService,
    IMapper mapper,
    UpdateTransferValidationValidator validator,
    ILogger<UpdateTransferValidationCommandHandler> logger) 
    : IRequestHandler<UpdateTransferValidationCommand, TransferValidationDto>
{
    public async Task<TransferValidationDto> Handle(
        UpdateTransferValidationCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("=== INICIANDO ACTUALIZACIÓN DE VALIDACIÓN DE TRANSFERENCIA ===");
        logger.LogInformation("ID: {Id}, Cliente: {CustomerName}, Monto: ${Amount:F2}", 
            request.Id,
            request.Request.CustomerName, 
            request.Request.TransactionAmount);

        // Validar el ID por separado
        if (request.Id <= 0)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("Id", "El ID de la validación de transferencia debe ser mayor a cero")
            });
        }

        // Validar el request usando FluentValidation
        await validator.ValidateAndThrowAsync(request.Request, cancellationToken);

        try
        {
            // Llamar al servicio de dominio para actualizar la validación
            var updated = await transferValidationService.UpdateAsync(
                idTransferValidation: request.Id,
                customerName: request.Request.CustomerName,
                transactionAmount: request.Request.TransactionAmount,
                transactionDate: request.Request.TransactionDate,
                transactionTime: request.Request.TransactionTime,
                status: request.Request.Status,
                confirmedBy: request.Request.ConfirmedBy,
                cancellationToken: cancellationToken);

            logger.LogInformation("✅ Validación de transferencia actualizada exitosamente con ID: {Id}", 
                updated.IdTransferValidation);
            logger.LogInformation("=========================================================");

            // Mapear la entidad al DTO de respuesta
            return mapper.Map<TransferValidationDto>(updated);
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
}
