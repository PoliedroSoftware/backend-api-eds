using Poliedro.Eds.Domain;
using Poliedro.Eds.Domain.DashboardPowerBI.TypeOfCollectionView.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.TypeOfCollectionView.DomainTypeOfCollectionView;

public interface ITypeOfCollectionViewGetAllTypeOfCollection
{
    Task<IEnumerable<TypeOfCollectionViewEntity>> GetAllAsync(PaginationParams paginationParams);
}
