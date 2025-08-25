using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.StrongBox.Entities;
using Poliedro.Eds.Domain.StrongBox.Repositories;
using Poliedro.Eds.Domain.StrongBox.ValueObjects;

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
            var last = await _strongBoxRepositoryGetLast.GetLastAsync(cancellationToken);
            var previousBalance = last?.Saldo ?? 0;

            type = type.Trim().ToUpperInvariant();

            decimal newBalance = 0;

            if (type == StrongBoxType.CORTE)
            {
                newBalance = previousBalance + ammount;
            }
            else if (type == StrongBoxType.RETIRO)
            {
                newBalance = previousBalance - ammount;
                if (newBalance < 0)
                {
                    throw new InvalidOperationException("No se puede hacer el retiro, esta en negativo!!!");
                }
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
            

            return entity;
        }
    }
}
