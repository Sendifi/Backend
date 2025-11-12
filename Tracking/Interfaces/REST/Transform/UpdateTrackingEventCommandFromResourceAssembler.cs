using System;
using Tracking.Domain.Commands;
using Tracking.Domain.Model.Aggregates;
using Tracking.Interfaces.REST.Resources;

namespace Tracking.Interfaces.REST.Transform
{
    public static class UpdateTrackingEventCommandFromResourceAssembler
    {
        public static UpdateTrackingEventCommand ToCommand(string id, UpdateTrackingEventResource r)
        {
            TrackingStatus? status = null;
            if (!string.IsNullOrWhiteSpace(r.Status))
            {
                if (!Enum.TryParse<TrackingStatus>(r.Status, true, out var st))
                    throw new ArgumentException("Invalid status");
                status = st;
            }
            return new UpdateTrackingEventCommand(
                id, r.ShipmentId, status, r.Description, r.Location, r.Timestamp, r.CourierReference
            );
        }
    }
}
