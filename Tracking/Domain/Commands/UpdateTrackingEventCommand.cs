using System;
using Tracking.Domain.Model.Aggregates;

namespace Tracking.Domain.Commands
{
    public record UpdateTrackingEventCommand(
        string Id,
        string? ShipmentId,
        TrackingStatus? Status,
        string? Description,
        string? Location,
        DateTime? Timestamp,
        string? CourierReference
    );
}
