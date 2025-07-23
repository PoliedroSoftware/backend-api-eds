
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
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

            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (AuditableEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = currentUser;
                    entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(AuditableEntity.CreatedBy)).IsModified = false;
                    entry.Property(nameof(AuditableEntity.CreatedAt)).IsModified = false;

                    entity.UpdatedBy = currentUser;
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
