using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracking.Domain.Model.Aggregates;
using Tracking.Domain.Repositories;

namespace Tracking.Application.Internal.InMemory
{
    public class InMemoryTrackingEventRepository : ITrackingEventRepository
    {
        public Task<IEnumerable<TrackingEvent>> ListAsync(string? shipmentId = null)
        {
            var q = InMemoryDataStore.TrackingEvents.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(shipmentId))
                q = q.Where(e => e.ShipmentId == shipmentId);
            return Task.FromResult(q);
        }

        public Task<TrackingEvent?> FindByIdAsync(string id) =>
            Task.FromResult(InMemoryDataStore.TrackingEvents.FirstOrDefault(e => e.Id == id));

        public Task AddAsync(TrackingEvent entity)
        {
            InMemoryDataStore.TrackingEvents.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TrackingEvent entity)
        {
            var idx = InMemoryDataStore.TrackingEvents.FindIndex(e => e.Id == entity.Id);
            if (idx >= 0) InMemoryDataStore.TrackingEvents[idx] = entity;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string id)
        {
            InMemoryDataStore.TrackingEvents.RemoveAll(e => e.Id == id);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string id) =>
            Task.FromResult(InMemoryDataStore.TrackingEvents.Any(e => e.Id == id));
    }
}
