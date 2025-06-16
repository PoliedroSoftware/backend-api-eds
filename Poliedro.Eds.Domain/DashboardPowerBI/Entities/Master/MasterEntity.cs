using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.Inventory.Dto.View;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Domain.DashboardPowerBI.Entities.Master;

public class MasterEntity
{
    public IEnumerable<CapacityEntity> Capacities { get; set; }
    public IEnumerable<CompartimentEntity> Compartiments { get; set; }
    public IEnumerable<EdsEntity> Eds { get; set; }
    public IEnumerable<InventoryListResponseDto> Inventory { get; set; }
    public IEnumerable<ProductEntity> Products { get; set; }
    public IEnumerable<ProviderEntity> Providers { get; set; }
    public IEnumerable<ShoppingEntity> Shoppings { get; set; }
    public IEnumerable<CourtListResponseDto> Court { get; set; }
    public IEnumerable<TypeOfCollectionEntity> TypeOfCollections { get; set; }
    
}
