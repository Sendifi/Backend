using backSendify.Tracking.Domain.Tracking.Enums;

namespace backSendify.Tracking.Application.Tracking.Requests;

public class TrackingEventUpdateRequest
{
    public TrackingStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? CourierReference { get; set; }
}
