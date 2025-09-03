using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Domain.StrongBox.Repositories;

public interface IStrongBoxRepositoryCreate
{
    Task CreateAsync(StrongBoxEntity entity, CancellationToken cancellationToken);
}
