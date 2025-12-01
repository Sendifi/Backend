using backSendify.Shared.Application.Common.Dtos;
using backSendify.Shipments.Domain.Shipments.Enums;

namespace backSendify.Shipments.Application.Shipments.Dtos;

public record ShipmentDto(
    Guid Id,
    string TrackingCode,
    PersonDto Sender,
    PersonDto Recipient,
    AddressDto OriginAddress,
    AddressDto DestinationAddress,
    double Weight,
    decimal Cost,
    ShipmentStatus Status,
    Guid? CourierId,
    Guid? DeliveryPersonId,
    DateTime? EstimatedDelivery,
    DateTime CreatedAt,
    DateTime UpdatedAt);
