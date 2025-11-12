using System.Threading.Tasks;
using Tracking.Domain.Commands;
using Tracking.Domain.Model.Aggregates;

namespace Tracking.Domain.Services
{
    public interface ITrackingEventCommandService
    {
        Task<TrackingEvent> Handle(CreateTrackingEventCommand command);
        Task<TrackingEvent?> Handle(UpdateTrackingEventCommand command);
        Task<bool> Handle(DeleteTrackingEventCommand command);
    }
}
