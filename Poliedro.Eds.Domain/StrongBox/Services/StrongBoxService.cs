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

public class StrongBoxService(IStrongBoxRepositoryGetLast _repo,
        IStrongBoxRepositoryCreate repoCreate,
        IStrongBoxRepositorySaveChanges repoSaveChanges) : IStrongBoxService
{
  
    public async Task<StrongBoxEntity> CreateAsync(StrongBoxEntity strongBoxEntity, CancellationToken cancellationToken)
    {
        if (strongBoxEntity.Ammount <= 0)
            throw new StrongBoxDomainException("El monto debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(strongBoxEntity.Type))
            throw new StrongBoxDomainException("El tipo de movimiento es requerido.");

         strongBoxEntity.Type.Trim().ToUpperInvariant();

        if (strongBoxEntity.Type != StrongBoxType.CORTE && strongBoxEntity.Type != StrongBoxType.RETIRO)
            throw new StrongBoxDomainException($"Tipo de movimiento no soportado: {strongBoxEntity.Type}");

        var last = await _repo.GetLastAsync(cancellationToken);
        var previousBalance = last?.Saldo ?? 0;

        decimal newBalance = previousBalance;

        switch (strongBoxEntity.Type)
        {
            case StrongBoxType.CORTE:
                // Siempre suma el monto al saldo anterior
                newBalance += strongBoxEntity.Ammount;
                break;
            case StrongBoxType.RETIRO:
                // Si el retiro es igual al saldo, el saldo queda en cero
                if (strongBoxEntity.Ammount == previousBalance)
                {
                    newBalance = 0;
                }
                else
                {
                    newBalance -= strongBoxEntity.Ammount;
                    if (newBalance < 0)
                        throw new StrongBoxDomainException("No se puede hacer el retiro, el saldo quedaría negativo.");
                }
                break;
        }



         await repoCreate.CreateAsync(strongBoxEntity, cancellationToken);

        return strongBoxEntity;


    }
}
