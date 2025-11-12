using System;
using System.Collections.Generic;
using Tracking.Domain.Model.Aggregates;

namespace Tracking.Application.Internal.InMemory
{
    public static class InMemoryDataStore
    {
        public static readonly List<TrackingEvent> TrackingEvents = new()
        {
            new TrackingEvent {
                Id = "1", ShipmentId = "1", Status = TrackingStatus.REGISTERED,
                Description = "Envío registrado", Location = "Bogotá",
                Timestamp = DateTime.Parse("2024-01-10T10:00:00Z")
            },
            new TrackingEvent {
                Id = "2", ShipmentId = "1", Status = TrackingStatus.IN_TRANSIT,
                Description = "En tránsito hacia Medellín", Location = "Bogotá",
                Timestamp = DateTime.Parse("2024-01-12T14:30:00Z"),
                CourierReference = "TRK-12345"
            }
        };
    }
}
