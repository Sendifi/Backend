using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Domain.Model.Aggregates;
using Tracking.Domain.Queries;

namespace Tracking.Domain.Services
{
    public interface ITrackingEventQueryService
    {
        Task<IEnumerable<TrackingEvent>> Handle(GetTrackingEventsQuery query);
        Task<TrackingEvent?> Handle(GetTrackingEventByIdQuery query);
    }
}
