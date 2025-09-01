<<<<<<< HEAD
namespace Poliedro.Eds.Domain.Audit.Entities
{
    public abstract class AuditableEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

=======
namespace Poliedro.Eds.Domain.Audit.Entities;

public abstract class AuditableEntity
{
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
>>>>>>> New-service-StrongBox
}
