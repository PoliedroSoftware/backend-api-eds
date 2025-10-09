using System.Net;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Bank.Errors;

public static class BankErrorBuilder
{
    public static Result<T, Error> BankNotFoundException<T>(int bankId)
    {
        return Error.CreateInstance(
            "BankNotFound", 
            $"Registro bancario con ID {bankId} no fue encontrado", 
            HttpStatusCode.NotFound);
    }

    public static Result<T, Error> BankCreationException<T>()
    {
        return Error.CreateInstance(
            "BankCreationFailed", 
            "Error al crear el registro bancario", 
            HttpStatusCode.InternalServerError);
    }

    public static Result<T, Error> InsufficientBalanceException<T>(double currentBalance, double requestedAmount)
    {
        return Error.CreateInstance(
            "InsufficientBalance", 
            $"Saldo insuficiente. Saldo actual: ${currentBalance:N2}, Monto solicitado: ${requestedAmount:N2}", 
            HttpStatusCode.BadRequest);
    }

    public static Result<T, Error> InvalidAccountException<T>(int accountId)
    {
        return Error.CreateInstance(
            "InvalidAccount", 
            $"La cuenta con ID {accountId} no es válida o no existe", 
            HttpStatusCode.BadRequest);
    }
}
