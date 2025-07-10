using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Shopping.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class ShoppingInventoryConfiguration
{
    public ShoppingInventoryConfiguration(EntityTypeBuilder<ShoppingInventoryEntity> builder)
    {
        builder.ToTable("inventory");
        builder.HasKey(x => x.IdInventory);
        builder.Property(x => x.IdInventory).HasColumnName("id_inventory");
        builder.Property(x => x.Date).HasColumnName("date");
        builder.Property(x => x.ReferenceType).HasColumnName("reference_type");
        builder.Property(x => x.ReferenceId).HasColumnName("reference_id");
    }
}