namespace backSendify.Shared.Domain.Common;

public abstract class AuditableEntity : EntityBase
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
