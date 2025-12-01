using backSendify.Tracking.Application.Tracking.Dtos;
using backSendify.Tracking.Application.Tracking.Requests;

namespace backSendify.Tracking.Application.Abstractions;

public interface ITrackingEventService
{
    Task<IEnumerable<TrackingEventDto>> GetAsync(TrackingEventQueryParameters query, CancellationToken cancellationToken);
    Task<TrackingEventDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TrackingEventDto> CreateAsync(TrackingEventCreateRequest request, CancellationToken cancellationToken);
    Task<TrackingEventDto?> UpdateAsync(Guid id, TrackingEventUpdateRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
