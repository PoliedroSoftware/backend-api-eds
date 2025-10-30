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

            // Set MySQL session variable for triggers to use
            // This ensures that triggers can access the application user
            if (!string.IsNullOrEmpty(currentUser))
            {
                // Use parameterized query to safely set the session variable
                await Database.ExecuteSqlRawAsync("SET @app_user = {0}", currentUser);
            }

            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (AuditableEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = currentUser;
                    entity.CreatedAt = DateTimeColombiaHelper.NowColombia(); // Cambiado a hora Colombia
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(AuditableEntity.CreatedBy)).IsModified = false;
                    entry.Property(nameof(AuditableEntity.CreatedAt)).IsModified = false;

                    entity.UpdatedBy = currentUser;
                    entity.UpdatedAt = DateTimeColombiaHelper.NowColombia(); // Cambiado a hora Colombia
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
