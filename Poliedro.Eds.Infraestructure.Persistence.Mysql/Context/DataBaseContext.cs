using Microsoft.EntityFrameworkCore;
using Poliedro.Eds.Domain.Account.Entities;
using Poliedro.Eds.Domain.Bank.Entities;
using Poliedro.Eds.Domain.Business.Entities;
using Poliedro.Eds.Domain.Capacity.Entities;
using Poliedro.Eds.Domain.Category.Entities;
using Poliedro.Eds.Domain.Compartiment.Entities;
using Poliedro.Eds.Domain.CompartimentCapacity.Entities;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Domain.CourtDispensersInventory.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.BusinessDashboardView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityDashboardView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.CapacityView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.CompartimentView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.EdsView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.ProductView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.ProviderView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.ShoppingProductView.Entities;
using Poliedro.Eds.Domain.DashboardPowerBI.TypeOfCollectionView.Entities;
using Poliedro.Eds.Domain.Dispensers.Entities;
using Poliedro.Eds.Domain.DispenserType.Entities;
using Poliedro.Eds.Domain.Eds.Entities;
using Poliedro.Eds.Domain.EdsTank.Entities;
using Poliedro.Eds.Domain.Expenditures.Entities;
using Poliedro.Eds.Domain.Hose.Entities;
using Poliedro.Eds.Domain.HoseHistory.Entities;
using Poliedro.Eds.Domain.Inventory.Entities;
using Poliedro.Eds.Domain.Island.Entities;
using Poliedro.Eds.Domain.Islander.Entities;
using Poliedro.Eds.Domain.OpenAI.Entities;
using Poliedro.Eds.Domain.Phone.Entities;
using Poliedro.Eds.Domain.Product.Entities;
using Poliedro.Eds.Domain.ProductType.Entities;
using Poliedro.Eds.Domain.Provider.Entities;
using Poliedro.Eds.Domain.RegisterShift.Entities;
using Poliedro.Eds.Domain.Shopping.Entities;
using Poliedro.Eds.Domain.ShoppingProduct.Entities;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;
using Poliedro.Eds.Domain.StrongBox.Entities;
using Poliedro.Eds.Domain.Tank.Entities;
using Poliedro.Eds.Domain.TransferValidation.Entities;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations.DashboardPowerBI;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.OpenAI.Configuration;


namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

public class DataBaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ProductTypeEntity> ProductType { get; set; }

    public DbSet<CourtEntity> Court { get; set; }

    public DbSet<BusinessEntity> Business { get; set; }

    public DbSet<PhoneEntity> Phone { get; set; }

    public DbSet<CapacityEntity> Capacity { get; set; }

    public DbSet<EdsEntity> Eds { get; set; }

    public DbSet<ProviderEntity> Provider { get; set; }

    public DbSet<ProductEntity> Product { get; set; }

    public DbSet<EdsTankEntity> EdsTank { get; set; }

    public DbSet<ExpendituresEntity> Expenditures { get; set; }

    public DbSet<TypeOfCollectionEntity> TypeOfCollection { get; set; }

    public DbSet<CompartimentCapacityEntity> CompartimentCapacity { get; set; }

    public DbSet<IslanderEntity> Islander { get; set; }

    public DbSet<IslandEntity> Island { get; set; }

    public DbSet<HoseEntity> Hose { get; set; }

    public DbSet<HoseHistoryEntity> HoseHistory { get; set; }

    public DbSet<DispensersEntity> Dispensers { get; set; }

    public DbSet<DispenserTypeEntity> DispenserType { get; set; }

    public DbSet<ShoppingEntity> Shopping { get; set; }

    public DbSet<ShoppingProductEntity> ShoppingProduct { get; set; }

    public DbSet<ShoppingProductInventoryEntity> ShoppingProductInventory { get; set; }

    public DbSet<TankEntity> Tank { get; set; }

    public DbSet<CompartimentEntity> Compartiment { get; set; }

    public DbSet<CourtDispensersInventoryEntity> CourtDispensersInventory { get; set; }

    public DbSet<CategoryEntity> Category { get; set; }

    public DbSet<ProductTypeEntity> ProductTypes { get; set; }

    public DbSet<InventoryEntity> Inventory { get; set; }

    public DbSet<BusinessViewEntity> BusinessView { get; set; }

    public DbSet<BusinessDashboardViewEntity> BusinessDashboardView { get; set; }

    public DbSet<CapacityDashboardViewEntity> CapacityDashboardView { get; set; }

    public DbSet<CapacityViewEntity> CapacityView { get; set; }

    public DbSet<EdsViewEntity> EdsView { get; set; }

    public DbSet<ProviderViewEntity> ProviderView { get; set; }

    public DbSet<ProductViewEntity> ProductView { get; set; }

    public DbSet<ShoppingProductViewEntity> ShoppingProductView { get; set; }

    public DbSet<CompartimentViewEntity> CompartimentView { get; set; }

    public DbSet<TypeOfCollectionViewEntity> TypeOfCollectionView { get; set; }

    public DbSet<OpenAIRequestEntity> OpenAIRequest { get; set; }

    public DbSet<OpenAIResponseEntity> OpenAIResponse { get; set; }

    public DbSet<StrongBoxEntity> StrongBox { get; set; }

    public DbSet<BankEntity> Bank { get; set; }

    public DbSet<AccountEntity> Account { get; set; }

    public DbSet<TransferValidationEntity> TransferValidation { get; set; }

    public DbSet<RegisterShiftEntity> RegisterShift { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        EntityConfiguration(modelBuilder);
    }

    private static void EntityConfiguration(ModelBuilder modelBuilder)
    {
        new ProductTypeConfiguration(modelBuilder.Entity<ProductTypeEntity>());
        new ProductConfiguration(modelBuilder.Entity<ProductEntity>());
        new EdsTankConfiguration(modelBuilder.Entity<EdsTankEntity>());
        new ExpendituresConfiguration(modelBuilder.Entity<ExpendituresEntity>());
        new CompartimentCapacityConfiguration(modelBuilder.Entity<CompartimentCapacityEntity>());
        new CourtConfiguration(modelBuilder.Entity<CourtEntity>());
        new BusinessConfiguration(modelBuilder.Entity<BusinessEntity>());
        new PhoneConfiguration(modelBuilder.Entity<PhoneEntity>());
        new CapacityConfiguration(modelBuilder.Entity<CapacityEntity>());
        new EdsConfiguration(modelBuilder.Entity<EdsEntity>());
        new ProviderConfiguration(modelBuilder.Entity<ProviderEntity>());
        new CourtDispenserConfiguration(modelBuilder.Entity<CourtDispenserEntity>());
        new CourtExpendituresConfiguration(modelBuilder.Entity<CourtExpenditureEntity>());
        new CourtTypeOfCollectionsConfiguration(modelBuilder.Entity<CourtTypeOfCollectionEntity>());
        new DocumentConfiguration(modelBuilder.Entity<DocumentEntity>());
        new IslanderConfiguration(modelBuilder.Entity<IslanderEntity>());
        new IslandConfiguration(modelBuilder.Entity<IslandEntity>());
        new DispensersConfiguration(modelBuilder.Entity<DispensersEntity>());
        new DispenserTypeConfiguration(modelBuilder.Entity<DispenserTypeEntity>());
        new HoseConfiguration(modelBuilder.Entity<HoseEntity>());
        new HoseHistoryConfiguration(modelBuilder.Entity<HoseHistoryEntity>());
        new TypeOfCollectionConfiguration(modelBuilder.Entity<TypeOfCollectionEntity>());
        new ShoppingConfiguration(modelBuilder.Entity<ShoppingEntity>());
        new ShoppingProductConfiguration(modelBuilder.Entity<ShoppingProductEntity>());

        new ShoppingProductInventoryConfiguration(modelBuilder.Entity<ShoppingProductInventoryEntity>());
        new TankConfiguration(modelBuilder.Entity<TankEntity>());
        new CompartimentConfiguration(modelBuilder.Entity<CompartimentEntity>());
        new CourtDispensersInventoryConfiguration(modelBuilder.Entity<CourtDispensersInventoryEntity>());
        new CategoryConfiguration(modelBuilder.Entity<CategoryEntity>());
        new InventoryConfiguration().Configure(modelBuilder.Entity<InventoryEntity>());
        new EdsViewConfiguration(modelBuilder.Entity<EdsViewEntity>());
        new BusinessViewConfiguration(modelBuilder.Entity<BusinessViewEntity>());
        new BusinessDashboardViewConfiguration(modelBuilder.Entity<BusinessDashboardViewEntity>());
        new CapacityDashboardViewConfiguration(modelBuilder.Entity<CapacityDashboardViewEntity>());
        new CapacityViewConfiguration(modelBuilder.Entity<CapacityViewEntity>());
        new ProductViewConfiguration(modelBuilder.Entity<ProductViewEntity>());
        new ProviderViewConfiguration(modelBuilder.Entity<ProviderViewEntity>());
        new CompartimentViewConfiguration(modelBuilder.Entity<CompartimentViewEntity>());
        new TypeOfCollectionViewConfiguration(modelBuilder.Entity<TypeOfCollectionViewEntity>());
        new ShoppingProductViewConfiguration(modelBuilder.Entity<ShoppingProductViewEntity>());
        new OpenAIRequestConfiguration(modelBuilder.Entity<OpenAIRequestEntity>());
        new OpenAIResponseConfiguration(modelBuilder.Entity<OpenAIResponseEntity>());
        new StrongBoxConfiguration(modelBuilder.Entity<StrongBoxEntity>());
        new BankConfiguration(modelBuilder.Entity<BankEntity>());
        new AccountConfiguration(modelBuilder.Entity<AccountEntity>());
        new TransferValidationConfiguration(modelBuilder.Entity<TransferValidationEntity>());
        new RegisterShiftConfiguration(modelBuilder.Entity<RegisterShiftEntity>());
    }
}
