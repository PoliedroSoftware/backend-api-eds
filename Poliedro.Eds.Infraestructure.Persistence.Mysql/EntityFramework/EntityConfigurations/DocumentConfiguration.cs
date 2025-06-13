using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.EntityFramework.EntityConfigurations;

public class DocumentConfiguration
{
    public DocumentConfiguration(EntityTypeBuilder<DocumentEntity> builder)
    {
        builder.ToTable("court_document");
        builder.HasKey(x => x.IdCourtDocument);
        builder.Property(x => x.IdCourtDocument).HasColumnName("idcourt_document");
        builder.Property(x => x.Descripcion).HasColumnName("descripcion");
        builder.Property(x => x.IdCourt).HasColumnName("id_court");

        builder.HasOne<CourtEntity>()
            .WithMany(x => x.CourtDocuments)
            .HasForeignKey(x => x.IdCourt);
    }
}
