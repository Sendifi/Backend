using System.ComponentModel.DataAnnotations;
using backSendify.Tracking.Domain.Tracking.Enums;

namespace backSendify.Tracking.Application.Tracking.Requests;

public class TrackingEventCreateRequest
{
    [Required]
    public Guid ShipmentId { get; set; }

    [Required]
    public TrackingStatus Status { get; set; } = TrackingStatus.Registered;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    public DateTime? Timestamp { get; set; }
    public string? CourierReference { get; set; }
}
