using backSendify.Shared.Domain.Common;
using backSendify.Shared.Domain.Common.ValueObjects;

namespace backSendify.Couriers.Domain.Couriers.Entities;

public class Courier : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public ContactInfo? Contact { get; set; }
    public PricingInfo? Pricing { get; set; }
    public decimal? CostPerKg { get; set; }
    public int? EstimatedDays { get; set; }
    public bool IsActive { get; set; } = true;
    public List<string> Services { get; set; } = new();
    public List<string> Coverage { get; set; } = new();
}
