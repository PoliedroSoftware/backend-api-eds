using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.TransferValidation.Dtos;
using Poliedro.Eds.Application.TransferValidation.Validation;
using Poliedro.Eds.Domain.TransferValidation.Exceptions;
using Poliedro.Eds.Domain.TransferValidation.Services;

namespace Poliedro.Eds.Application.TransferValidation.Commands.CreateTransferValidation;

public class CreateTransferValidationCommandHandler(
    ITransferValidationService transferValidationService,
    IMapper mapper,
    TransferValidationCreateValidator validator,
    ILogger<CreateTransferValidationCommandHandler> logger) 
    : IRequestHandler<CreateTransferValidationCommand, TransferValidationDto>
{
    public async Task<TransferValidationDto> Handle(
        CreateTransferValidationCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("=== INICIANDO CREACIÓN DE VALIDACIÓN DE TRANSFERENCIA ===");
        logger.LogInformation("UniqueId: {UniqueId}, Cliente: {CustomerName}, Monto: ${Amount:F2}", 
            request.Request.UniqueId, 
            request.Request.CustomerName, 
            request.Request.TransactionAmount);

        // Validar el request usando FluentValidation
        await validator.ValidateAndThrowAsync(request.Request, cancellationToken);

        try
        {
            // Llamar al servicio de dominio para crear la validación
            var created = await transferValidationService.CreateAsync(
                uniqueId: request.Request.UniqueId,
                customerName: request.Request.CustomerName,
                transactionAmount: request.Request.TransactionAmount,
                transactionDate: request.Request.TransactionDate,
                transactionTime: request.Request.TransactionTime,
                status: request.Request.Status,
                confirmedBy: request.Request.ConfirmedBy,
                cancellationToken: cancellationToken);

            logger.LogInformation("✅ Validación de transferencia creada exitosamente con ID: {Id}", 
                created.IdTransferValidation);
            logger.LogInformation("=========================================================");

            // Mapear la entidad al DTO de respuesta
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
}
