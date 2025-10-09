using Poliedro.Eds.Domain.Bank.Entities;
using Poliedro.Eds.Domain.Bank.Repositories;
using Poliedro.Eds.Domain.Bank.ValueObjects;
using Poliedro.Eds.Domain.Bank.Exceptions;

namespace Poliedro.Eds.Domain.Bank.Services;

public class BankService(
    IBankRepositoryGetLast bankRepositoryGetLast,
    IBankRepositoryCreate bankRepositoryCreate) : IBankService
{
    public async Task<BankEntity> CreateAsync(
        int idAccount,
        int idEds,
        int? idCourt,
        string moviment,
        double ammount,
        string note,
        CancellationToken cancellationToken)
    {
        if (ammount <= 0)
            throw new BankDomainException("El monto debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(moviment))
            throw new BankDomainException("El tipo de movimiento es requerido.");

        var normalMoviment = moviment.Trim().ToUpperInvariant();
        if (normalMoviment != BankMovementType.CORTE && 
            normalMoviment != BankMovementType.RETIRO && 
            normalMoviment != BankMovementType.DEPOSITO)
            throw new BankDomainException($"Tipo de movimiento no soportado: {normalMoviment}");

        if (string.IsNullOrWhiteSpace(note))
            throw new BankDomainException("La nota es requerida.");

        // Obtener el último saldo de la cuenta específica
        var lastEntry = await bankRepositoryGetLast.GetLastByAccountAsync(idAccount, cancellationToken);
        var previousBalance = lastEntry?.Balance ?? 0.0;

        double newBalance = previousBalance;

        switch (normalMoviment)
        {
            case BankMovementType.CORTE:
            case BankMovementType.DEPOSITO:
                // Suma el monto al saldo anterior
                newBalance = previousBalance + ammount;
                break;
            case BankMovementType.RETIRO:
                // Si el retiro es mayor al saldo, arroja excepción
                if (ammount > previousBalance)
                    throw new BankDomainException("No se puede hacer el retiro, el monto es mayor al saldo en la cuenta.");

                newBalance = previousBalance - ammount;
                break;
        }

        var entity = new BankEntity(
            idAccount: idAccount,
            idEds: idEds,
            idCourt: idCourt,
            moviment: normalMoviment,
            note: note,
            ammount: ammount,
            balance: newBalance
        );

        await bankRepositoryCreate.CreateAsync(entity, cancellationToken);

        return entity;
    }
}
