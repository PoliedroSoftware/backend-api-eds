using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.ShoppingProductInventory.Entities;


namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations
{
    public class ShoppingProductInventoryConfiguration
    {
        public ShoppingProductInventoryConfiguration(EntityTypeBuilder<ShoppingProductInventoryEntity> builder)
        {
            builder.ToTable("shopping_produc_inventory");
            builder.HasKey(x => x.IdShoppingProductInventory);
            builder.Property(x => x.IdShoppingProductInventory).HasColumnName("id_shopping_produc_inventorycol");
            builder.Property(x => x.IdShoppingProduct).HasColumnName("id_shopping_product");
            builder.Property(x => x.IdInventory).HasColumnName("idinventory");
            
        }

    }
}
