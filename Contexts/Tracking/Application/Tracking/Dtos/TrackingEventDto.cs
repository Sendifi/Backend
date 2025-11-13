using backSendify.Tracking.Domain.Tracking.Enums;

namespace backSendify.Tracking.Application.Tracking.Dtos;

public record TrackingEventDto(
    Guid Id,
    Guid ShipmentId,
    TrackingStatus Status,
    string Description,
    string Location,
    DateTime Timestamp,
    string? CourierReference);
