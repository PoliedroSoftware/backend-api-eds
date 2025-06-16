using Poliedro.Eds.Domain.Common.Pagination;
using Poliedro.Eds.Domain.Capacity.DomainCapacity;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Eds.DomainEds;
using Poliedro.Eds.Domain.Product.DomainProduct;
using Poliedro.Eds.Domain.Provider.DomainProvider;
using Poliedro.Eds.Domain.Shopping.DomainShopping;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
using Poliedro.Eds.Domain.Inventory.DomainService;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.DashboardPowerBI.Entities.Master;
using Poliedro.Eds.Domain.DashboardPowerBI.DomainDashboardPowerBI;
using Poliedro.Eds.Application.Ports.Redis;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.DashboardPowerBI.DomainService.lmpl;

public class MasterGetAllService(
    IRedisService redisService,
    ICapacityGetAllService capacityGetAllService,
    ICompartimentGetAllCompartiment compartimentGetAllCompartiment,
    IEdsGetAllService edsGetAllService,
    IProductGetAllProduct productGetAllProduct,
    IProviderGetAllService providerGetAllService,
    IShoppingGetAllShopping shoppingGetAllShopping,
    ITypeOfCollectionGetAllTypeOfCollection typeOfCollectionGetAllTypeOfCollection,
    IInventoryListDomainService inventoryListDomainService,
    ICourtListDomainService courtListDomainService) : IMasterGetAllService
{
    public async Task<MasterEntity> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
    {

        string cacheKey = $"master:{paginationParams.PageNumber}:{paginationParams.PageSize}";
        var cachedData = await redisService.GetCacheAsync<MasterEntity>(cacheKey);
        if (cachedData is not null) return cachedData;

        var masterEntity = new MasterEntity
        {
            Capacities = await capacityGetAllService.GetAllAsync(paginationParams),
            Compartiments = await compartimentGetAllCompartiment.GetAllAsync(paginationParams),
            Eds = await edsGetAllService.GetAllAsync(paginationParams),
            Products = await productGetAllProduct.GetAllAsync(paginationParams),
            Providers = await providerGetAllService.GetAllAsync(paginationParams),
            Shoppings = await shoppingGetAllShopping.GetAllAsync(paginationParams),
            TypeOfCollections = await typeOfCollectionGetAllTypeOfCollection.GetAllAsync(paginationParams),
            Inventory = await inventoryListDomainService.GetAllAsync(paginationParams),
            Court = await courtListDomainService.GetAllAsync(paginationParams)
        };

        await redisService.SetCacheAsync(cacheKey, masterEntity, TimeSpan.FromMinutes(1440));

        return masterEntity;
    }
}
