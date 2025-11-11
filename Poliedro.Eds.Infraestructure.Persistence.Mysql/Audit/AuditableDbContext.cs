using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Utils;
using Poliedro.Eds.Domain.Audit.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Audit
{
    public class AuditableDbContext(
        DbContextOptions options,
        IHttpContextAccessor httpContextAccessor) : DataBaseContext(options)
    {
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUser = httpContextAccessor.HttpContext?.Items["identifiername"]?.ToString();
            var currentTimestamp = DateTimeColombiaHelper.NowColombia();

            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (AuditableEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = currentUser;
                    entity.CreatedAt = currentTimestamp;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(AuditableEntity.CreatedBy)).IsModified = false;
                    entry.Property(nameof(AuditableEntity.CreatedAt)).IsModified = false;

                    entity.UpdatedBy = currentUser;
                    entity.UpdatedAt = currentTimestamp;
                }
            }

            // Set MySQL session variables for triggers to use
            // This ensures triggers that insert into history tables use the same user and timestamp
            if (!string.IsNullOrEmpty(currentUser))
            {
                await Database.ExecuteSqlRawAsync(
                    "SET @app_user = {0}, @app_timestamp = {1}",
                    currentUser,
                    currentTimestamp.ToString("yyyy-MM-dd HH:mm:ss")
                );
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
