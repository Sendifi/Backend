using Tracking.Domain.Model.Aggregates;

namespace Tracking.Domain.Commands
{
    public record CreateTrackingEventCommand(
        string ShipmentId,
        TrackingStatus Status,
        string Description,
        string? Location,
        string? CourierReference
    );
}
