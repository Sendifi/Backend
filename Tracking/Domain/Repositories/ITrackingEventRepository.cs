using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Domain.Model.Aggregates;

namespace Tracking.Domain.Repositories
{
    public interface ITrackingEventRepository
    {
        Task<IEnumerable<TrackingEvent>> ListAsync(string? shipmentId = null);
        Task<TrackingEvent?> FindByIdAsync(string id);
        Task AddAsync(TrackingEvent entity);
        Task UpdateAsync(TrackingEvent entity);
        Task RemoveAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}
