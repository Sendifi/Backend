using System;

namespace Tracking.Domain.Model.Aggregates
{
    public class TrackingEvent
    {
        public string Id { get; set; } = default!;
        public string ShipmentId { get; set; } = default!;
        public TrackingStatus Status { get; set; }
        public string Description { get; set; } = default!;
        public string? Location { get; set; }
        public DateTime Timestamp { get; set; }
        public string? CourierReference { get; set; }
    }
}
