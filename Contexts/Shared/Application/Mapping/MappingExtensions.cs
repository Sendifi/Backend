using backSendify.Shared.Application.Common.Dtos;
using backSendify.Couriers.Application.Couriers.Dtos;
using backSendify.Delivery.Application.DeliveryPersons.Dtos;
using backSendify.Shipments.Application.Shipments.Dtos;
using backSendify.Tracking.Application.Tracking.Dtos;
using backSendify.Users.Application.Users.Dtos;
using backSendify.Shared.Domain.Common.ValueObjects;
using backSendify.Couriers.Domain.Couriers.Entities;
using backSendify.Delivery.Domain.DeliveryPersons.Entities;
using backSendify.Shipments.Domain.Shipments.Entities;
using backSendify.Tracking.Domain.Tracking.Entities;
using backSendify.Users.Domain.Users.Entities;

namespace backSendify.Shared.Application.Mapping;

internal static class MappingExtensions
{
    public static ShipmentDto ToDto(this Shipment entity) => new(
        entity.Id,
        entity.TrackingCode,
        entity.Sender.ToDto(),
        entity.Recipient.ToDto(),
        entity.OriginAddress.ToDto(),
        entity.DestinationAddress.ToDto(),
        entity.Weight,
        entity.Cost,
        entity.Status,
        entity.CourierId,
        entity.DeliveryPersonId,
        entity.EstimatedDelivery,
        entity.CreatedAt,
        entity.UpdatedAt);

    public static DeliveryPersonDto ToDto(this DeliveryPerson entity) => new(
        entity.Id,
        entity.Name,
        entity.Code,
        entity.Phone,
        entity.IsActive,
        entity.AssignedShipments.AsReadOnly(),
        entity.CreatedAt,
        entity.UpdatedAt);

    public static TrackingEventDto ToDto(this TrackingEvent entity) => new(
        entity.Id,
        entity.ShipmentId,
        entity.Status,
        entity.Description,
        entity.Location,
        entity.Timestamp,
        entity.CourierReference);

    public static CourierDto ToDto(this Courier entity) => new(
        entity.Id,
        entity.Name,
        entity.Contact?.ToDto(),
        entity.Pricing?.ToDto(),
        entity.CostPerKg,
        entity.EstimatedDays,
        entity.IsActive,
        entity.Services.AsReadOnly(),
        entity.Coverage.AsReadOnly(),
        entity.CreatedAt,
        entity.UpdatedAt);

    public static UserDto ToDto(this ApplicationUser entity) => new(
        entity.Id,
        entity.Email,
        entity.Name,
        entity.Username,
        entity.Role,
        entity.Avatar,
        entity.IsActive,
        entity.CreatedAt,
        entity.UpdatedAt);

    private static PersonDto ToDto(this PersonInfo value) => new(value.Name, value.Email, value.Phone);
    private static AddressDto ToDto(this Address value) => new(value.Street, value.City, value.State, value.ZipCode, value.Country);
    private static ContactDto ToDto(this ContactInfo value) => new(value.Email, value.Phone, value.Website);
    private static PricingDto ToDto(this PricingInfo value) => new(value.BaseCost, value.PerKg);
}
