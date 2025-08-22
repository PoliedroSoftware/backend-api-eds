using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poliedro.Eds.Domain.OpenAI.Entities;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.OpenAI.Configuration;

public class OpenAIRequestConfiguration
{
    public OpenAIRequestConfiguration(EntityTypeBuilder<OpenAIRequestEntity> builder)
    {
        builder.ToTable("openai_requests");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserMessage)
            .HasColumnName("user_message")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.Model)
            .HasColumnName("model")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.SystemMessage)
            .HasColumnName("system_message")
            .HasColumnType("varchar(1000)")
            .HasMaxLength(1000);

        builder.Property(x => x.MaxTokens)
            .HasColumnName("max_tokens")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.Temperature)
            .HasColumnName("temperature")
            .HasColumnType("double")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .HasColumnType("varchar(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        // Index for better query performance
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_openai_requests_user_id");

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_openai_requests_created_at");
    }
}