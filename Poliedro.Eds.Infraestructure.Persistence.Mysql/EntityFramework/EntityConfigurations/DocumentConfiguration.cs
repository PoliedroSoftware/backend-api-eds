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
        builder.Property(x => x.IdCourtDocument).HasColumnName("idcourt_document")
        .ValueGeneratedOnAdd();

        // Hacer Descripcion opcional para evitar guardar imágenes base64 pesadas
        builder.Property(x => x.Descripcion).HasColumnName("descripcion")
          .HasColumnType("LONGTEXT")
          .IsRequired(false); // Cambiado de IsRequired() a IsRequired(false)
     
        builder.Property(x => x.DocumentName).HasColumnName("document_name")
            .HasMaxLength(100);
   
        builder.Property(x => x.IdCourt).HasColumnName("id_court");

     
        builder.HasOne<CourtEntity>()
            .WithMany(x => x.CourtDocuments)
            .HasForeignKey(x => x.IdCourt)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
