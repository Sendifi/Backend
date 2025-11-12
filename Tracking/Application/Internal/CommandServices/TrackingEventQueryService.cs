using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Domain.Model.Aggregates;
using Tracking.Domain.Queries;
using Tracking.Domain.Repositories;
using Tracking.Domain.Services;

namespace Tracking.Application.Internal.QueryServices
{
    public class TrackingEventQueryService : ITrackingEventQueryService
    {
        private readonly ITrackingEventRepository _repo;
        public TrackingEventQueryService(ITrackingEventRepository repo) => _repo = repo;

        public Task<IEnumerable<TrackingEvent>> Handle(GetTrackingEventsQuery q) =>
            _repo.ListAsync(q.ShipmentId);

        public Task<TrackingEvent?> Handle(GetTrackingEventByIdQuery q) =>
            _repo.FindByIdAsync(q.Id);
    }
}
