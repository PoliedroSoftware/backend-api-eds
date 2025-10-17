using Poliedro.Eds.Domain.Common.Results.Errors;
using System.Net;

namespace Poliedro.Eds.Application.TransferValidation.Errors;

public static class TransferValidationErrorBuilder
{
    public static Error TransferValidationNotFound(int id) =>
        Error.CreateInstance(
            "TransferValidation.NotFound",
            $"No se encontró la validación de transferencia con ID {id}",
            HttpStatusCode.NotFound);

    public static Error DuplicateUniqueId(string uniqueId) =>
        Error.CreateInstance(
            "TransferValidation.DuplicateUniqueId",
            $"Ya existe una validación de transferencia con el ID único: {uniqueId}",
            HttpStatusCode.Conflict);

    public static Error InvalidStatus(string status) =>
        Error.CreateInstance(
            "TransferValidation.InvalidStatus",
            $"El estado '{status}' no es válido",
            HttpStatusCode.BadRequest);

    public static Error InvalidAmount() =>
        Error.CreateInstance(
            "TransferValidation.InvalidAmount",
            "El monto de la transacción debe ser mayor a cero",
            HttpStatusCode.BadRequest);

    public static Error RequiredConfirmedBy() =>
        Error.CreateInstance(
            "TransferValidation.RequiredConfirmedBy",
            "Debe especificar quién confirmó la transacción cuando el estado es CONFIRMADA",
            HttpStatusCode.BadRequest);

    public static Error FutureDate() =>
        Error.CreateInstance(
            "TransferValidation.FutureDate",
            "La fecha de la transacción no puede ser futura",
            HttpStatusCode.BadRequest);

    public static Error CreationFailed(string reason) =>
        Error.CreateInstance(
            "TransferValidation.CreationFailed",
            $"No se pudo crear la validación de transferencia: {reason}",
            HttpStatusCode.InternalServerError);
}
