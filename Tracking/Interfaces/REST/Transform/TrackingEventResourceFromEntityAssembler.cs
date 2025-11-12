using Tracking.Domain.Model.Aggregates;
using Tracking.Interfaces.REST.Resources;

namespace Tracking.Interfaces.REST.Transform
{
    public static class TrackingEventResourceFromEntityAssembler
    {
        public static TrackingEventResource ToResource(TrackingEvent e) => new()
        {
            Id = e.Id,
            ShipmentId = e.ShipmentId,
            Status = e.Status.ToString(),
            Description = e.Description,
            Location = e.Location,
            Timestamp = e.Timestamp,
            CourierReference = e.CourierReference
        };
    }
}
