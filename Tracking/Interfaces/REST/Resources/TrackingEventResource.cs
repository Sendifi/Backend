using System;

namespace Tracking.Interfaces.REST.Resources
{
    public class TrackingEventResource
    {
        public string Id { get; set; } = default!;
        public string ShipmentId { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Location { get; set; }
        public DateTime Timestamp { get; set; }
        public string? CourierReference { get; set; }
    }

    public class CreateTrackingEventResource
    {
        public string ShipmentId { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Location { get; set; }
        public string? CourierReference { get; set; }
    }

    public class UpdateTrackingEventResource
    {
        public string? ShipmentId { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? CourierReference { get; set; }
    }
}
