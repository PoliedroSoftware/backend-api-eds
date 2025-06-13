using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Poliedro.Eds.Application.Business.AutoMappers;
using Poliedro.Eds.Application.Capacity.AutoMappers;
using Poliedro.Eds.Application.Category.AutoMappers;
using Poliedro.Eds.Application.Common.Behaviors;
using Poliedro.Eds.Application.Compartiment.AutoMappers;
using Poliedro.Eds.Application.CompartimentCapacity.AutoMappers;
using Poliedro.Eds.Application.Court.AutoMappers;
using Poliedro.Eds.Application.CourtDispensersInventory.AutoMappers;
using Poliedro.Eds.Application.Dispensers.AutoMappers;
using Poliedro.Eds.Application.DispenserType.AutoMappers;
using Poliedro.Eds.Application.Eds.AutoMappers;
using Poliedro.Eds.Application.EdsTank.AutoMappers;
using Poliedro.Eds.Application.Expenditures.AutoMappers;
using Poliedro.Eds.Application.Hose.AutoMappers;
using Poliedro.Eds.Application.HoseHistory.AutoMappers;
using Poliedro.Eds.Application.Island.AutoMappers;
using Poliedro.Eds.Application.Islander.AutoMappers;
using Poliedro.Eds.Application.Product.AutoMappers;
using Poliedro.Eds.Application.ProductCompartiment.AutoMappers;
using Poliedro.Eds.Application.ProductType.AutoMappers;
using Poliedro.Eds.Application.Provider.AutoMappers;
using Poliedro.Eds.Application.Shopping.AutoMappers;
using Poliedro.Eds.Application.ShoppingProduct.AutoMappers;
using Poliedro.Eds.Application.ShoppingProductInventory.AutoMappers;
using Poliedro.Eds.Application.Tank.AutoMappers;
using Poliedro.Eds.Application.TypeOfCollection.AutoMappers;
using System.Reflection;

namespace Poliedro.Eds.Application;

public static class DependencyInjectionService
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        #region Mappers
        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new CourtMapper());
            config.AddProfile(new BusinessMapper());
            config.AddProfile(new PaginationBusinessMapper());
            config.AddProfile(new CapacityMapper());
            config.AddProfile(new PaginationCapacityMapper());
            config.AddProfile(new EdsMapper());
            config.AddProfile(new PaginationEdsMapper());
            config.AddProfile(new ProviderMapper());
            config.AddProfile(new PaginationProviderMapper());
            config.AddProfile(new PaginationDispenserMapper());
            config.AddProfile(new DispensersMapper());
            config.AddProfile(new ProductType.AutoMappers.PaginationMapper());
            config.AddProfile(new ProductTypeMapper());
            config.AddProfile(new Expenditures.AutoMappers.PaginationMapper());
            config.AddProfile(new ExpendituresMapper());
            config.AddProfile(new TypeOfCollection.AutoMappers.PaginationMapper());
            config.AddProfile(new TypeOfCollectionMapper());
            config.AddProfile(new PaginationDispenserTypeMapper());
            config.AddProfile(new DispenserTypeMapper());
            config.AddProfile(new Product.AutoMappers.PaginationMapper());
            config.AddProfile(new ProductMapper());
            config.AddProfile(new ProductCompartiment.AutoMappers.PaginationMapper());
            config.AddProfile(new ProductCompartimentMapper());
            config.AddProfile(new EdsTank.AutoMappers.PaginationMapper());
            config.AddProfile(new EdsTankMapper());
            config.AddProfile(new CompartimentCapacity.AutoMappers.PaginationMapper());
            config.AddProfile(new CompartimentCapacityMapper());
            config.AddProfile(new IslanderMapper());
            config.AddProfile(new PaginationIslanderMapper());
            config.AddProfile(new IslandMapper());
            config.AddProfile(new PaginationIslandMapper());
            config.AddProfile(new HoseMapper());
            config.AddProfile(new PaginationHoseMapper());
            config.AddProfile(new HoseHistoryMapper());
            config.AddProfile(new PaginationHoseHistoryMapper());
            config.AddProfile(new ShoppingMapper());
            config.AddProfile(new PaginationShoppingMapper());
            config.AddProfile(new ShoppingProductMapper());
            config.AddProfile(new PaginationShoppingProductMapper());
            config.AddProfile(new ShoppingProductInventoryMapper());
            config.AddProfile(new PaginationShoppingProductInventoryMapper());
            config.AddProfile(new TankMapper());
            config.AddProfile(new PaginationTankMapper());
            config.AddProfile(new CompartimentMapper());
            config.AddProfile(new PaginationCompartimentMapper());
            config.AddProfile(new CourtDispensersInventoryMapper());
            config.AddProfile(new PaginationCourtDispensersInventoryMapper());
            config.AddProfile(new CategoryMapper());
            config.AddProfile(new PaginationCategoryMapper()); 
        });
        services.AddSingleton(mapper.CreateMapper());
        #endregion

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviour<,>)
        );
        
        return services;
    }
}