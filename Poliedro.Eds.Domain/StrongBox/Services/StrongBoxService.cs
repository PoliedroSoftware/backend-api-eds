using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.Entities;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Domain.StrongBox.ValueObjects;
using Poliedro.Eds.Domain.StrongBox.Exceptions;

namespace Poliedro.Eds.Domain.StrongBox.Services;

public class StrongBoxService(IStrongBoxRepositoryGetLast _repoLast,
        IStrongBoxRepositoryCreate repoCreate) : IStrongBoxService
{
  
    public async Task<StrongBoxEntity> CreateAsync(
        long? idCorte,
        string type,
        double ammount,
        string? note,
        CancellationToken cancellationToken)
    {
        
        if (ammount <= 0)
            throw new StrongBoxDomainException("El monto debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(type))
            throw new StrongBoxDomainException("El tipo de movimiento es requerido.");

        var normalType = type.Trim().ToUpperInvariant();
        if (normalType != StrongBoxType.CORTE && normalType != StrongBoxType.RETIRO)
            throw new StrongBoxDomainException($"Tipo de movimiento no soportado: {normalType}");

        var last = await _repoLast.GetLastAsync(cancellationToken);
        var previousBalance = last?.Saldo ?? 0.0;

        double newBalance = previousBalance;

        switch (normalType)
        {
            case StrongBoxType.CORTE:
                // Siempre suma el monto al saldo anterior
                newBalance = previousBalance + ammount;
                break;
            case StrongBoxType.RETIRO:
                // Si el retiro es igual al saldo, el saldo queda en cero
                if (ammount > previousBalance)
                    throw new StrongBoxDomainException("No se puede hacer el retiro, el monto es mayor al saldo en caja fuerte!! ");

                if (string.IsNullOrWhiteSpace(note))
                {
                    note = $"Retiro de Caja por valor de ${ammount:N2}";
                }

                if (idCorte is null or 0L)
                {
                    idCorte = null;
                }

                newBalance = previousBalance - ammount;
                break;
        }
        var entity = new StrongBoxEntity(
            idCorte: idCorte,
            type: normalType,      
            ammount: ammount,
            saldo: newBalance,
            note: note
        );

        await repoCreate.CreateAsync(entity, cancellationToken);

        return entity;
    }
}
