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
        private readonly IStrongBoxRepository _strongBoxRepository;
        public StrongBoxService(IStrongBoxRepository repo)
        {
            _strongBoxRepository = repo;
        }
        public async Task<StrongBoxEntity> CreateAsync(DateTime dateTime, long? idCorte, string type, decimal ammount, string? note, string createdBy, CancellationToken cancellationToken)
        {
            var entity = await _strongBoxRepository.GetLastAsync(cancellationToken);
            var last = await _strongBoxRepository.GetLastAsync(cancellationToken);
            var previousBalance = last?.Saldo ?? 0;

            decimal newBalance = entity.Type switch
            {
                StrongBoxType.CORTE => previousBalance + entity.Ammount,
                StrongBoxType.RETIRO => previousBalance + entity.Ammount,
                _ => throw new InvalidOperationException("Tipo Invalido")
            };
            if (newBalance < 0)
            {
                throw new InvalidOperationException("No se puede hacer el retiro, esta en negativo!!!");
            }

            entity.SetSaldo(newBalance, createdBy);
            await _strongBoxRepository.AddAsync(entity, cancellationToken);
            await _strongBoxRepository.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
