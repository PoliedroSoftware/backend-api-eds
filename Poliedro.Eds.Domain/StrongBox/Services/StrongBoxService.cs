using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.Entities;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Domain.StrongBox.ValueObjects;
using Poliedro.Eds.Domain.StrongBox.Exceptions;

namespace Poliedro.Eds.Domain.StrongBox.Services
{
    public class StrongBoxService : IStrongBoxService
    {
        private readonly IStrongBoxRepositoryGetLast _strongBoxRepositoryGetLast;
        private readonly IStrongBoxRepositoryCreate _strongBoxRepositoryCreate;
        private readonly IStrongBoxRepositorySaveChanges _strongBoxRepositorySaveChanges;
        public StrongBoxService(IStrongBoxRepositoryGetLast repo,
            IStrongBoxRepositoryCreate repoCreate,
            IStrongBoxRepositorySaveChanges repoSaveChanges)
        {
            _strongBoxRepositoryGetLast = repo;
            _strongBoxRepositoryCreate = repoCreate;
            _strongBoxRepositorySaveChanges = repoSaveChanges;
        }
        public async Task<StrongBoxEntity> CreateAsync(DateTime dateTime, long? idCorte, string type, decimal ammount, string? note, CancellationToken cancellationToken)
        {
            if (ammount <= 0)
                throw new StrongBoxDomainException("El monto debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(type))
                throw new StrongBoxDomainException("El tipo de movimiento es requerido.");

            type = type.Trim().ToUpperInvariant();

            if (type != StrongBoxType.CORTE && type != StrongBoxType.RETIRO)
                throw new StrongBoxDomainException($"Tipo de movimiento no soportado: {type}");

            var last = await _strongBoxRepositoryGetLast.GetLastAsync(cancellationToken);
            var previousBalance = last?.Saldo ?? 0;

            decimal newBalance = previousBalance;

            switch (type)
            {
                case StrongBoxType.CORTE:
                    // Siempre suma el monto al saldo anterior
                    newBalance += ammount;
                    break;
                case StrongBoxType.RETIRO:
                    // Si el retiro es igual al saldo, el saldo queda en cero
                    if (ammount == previousBalance)
                    {
                        newBalance = 0;
                    }
                    else
                    {
                        newBalance -= ammount;
                        if (newBalance < 0)
                            throw new StrongBoxDomainException("No se puede hacer el retiro, el saldo quedaría negativo.");
                    }
                    break;
            }

            var entity = new StrongBoxEntity(
                dateTime: dateTime,
                idCorte: idCorte,
                type: type,
                ammount: ammount,
                saldo: newBalance,
                note: note
            );

            await _strongBoxRepositoryCreate.CreateAsync(entity, cancellationToken);
            // Si se requiere guardar cambios explícitamente, descomentar:
            // await _strongBoxRepositorySaveChanges.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
