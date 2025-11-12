using System;
using System.Threading.Tasks;
using Tracking.Domain.Commands;
using Tracking.Domain.Model.Aggregates;
using Tracking.Domain.Repositories;
using Tracking.Domain.Services;

namespace Tracking.Application.Internal.CommandServices
{
    public class TrackingEventCommandService : ITrackingEventCommandService
    {
        private readonly ITrackingEventRepository _repo;
        public TrackingEventCommandService(ITrackingEventRepository repo) => _repo = repo;

        public async Task<TrackingEvent> Handle(CreateTrackingEventCommand c)
        {
            var entity = new TrackingEvent
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                ShipmentId = c.ShipmentId,
                Status = c.Status,
                Description = c.Description,
                Location = c.Location,
                Timestamp = DateTime.UtcNow,
                CourierReference = c.CourierReference
            };
            await _repo.AddAsync(entity);
            return entity;
        }

        public async Task<TrackingEvent?> Handle(UpdateTrackingEventCommand c)
        {
            var current = await _repo.FindByIdAsync(c.Id);
            if (current is null) return null;

            current.ShipmentId = c.ShipmentId ?? current.ShipmentId;
            current.Status = c.Status ?? current.Status;
            current.Description = c.Description ?? current.Description;
            current.Location = c.Location ?? current.Location;
            current.Timestamp = c.Timestamp ?? current.Timestamp;
            current.CourierReference = c.CourierReference ?? current.CourierReference;

            await _repo.UpdateAsync(current);
            return current;
        }

        public async Task<bool> Handle(DeleteTrackingEventCommand c)
        {
            if (!await _repo.ExistsAsync(c.Id)) return false;
            await _repo.RemoveAsync(c.Id);
            return true;
        }
    }
}
