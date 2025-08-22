using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.OpenAI.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.OpenAI.Configuration;

public class OpenAIResponseConfiguration
{
    public OpenAIResponseConfiguration(EntityTypeBuilder<OpenAIResponseEntity> builder)
    {
        builder.ToTable("openai_responses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.RequestId)
            .HasColumnName("request_id")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.ResponseContent)
            .HasColumnName("response_content")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.Model)
            .HasColumnName("model")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.TokensUsed)
            .HasColumnName("tokens_used")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.FinishReason)
            .HasColumnName("finish_reason")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ProcessingTimeMs)
            .HasColumnName("processing_time_ms")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        // Foreign key relationship
        builder.HasOne(x => x.Request)
            .WithMany()
            .HasForeignKey(x => x.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for better query performance
        builder.HasIndex(x => x.RequestId)
            .HasDatabaseName("IX_openai_responses_request_id");

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_openai_responses_created_at");
    }
}