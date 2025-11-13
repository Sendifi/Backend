using backSendify.Shared.Domain.Common;
using backSendify.Tracking.Domain.Tracking.Enums;

namespace backSendify.Tracking.Domain.Tracking.Entities;

public class TrackingEvent : EntityBase
{
    public Guid ShipmentId { get; set; }
    public TrackingStatus Status { get; set; } = TrackingStatus.Registered;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? CourierReference { get; set; }
}
