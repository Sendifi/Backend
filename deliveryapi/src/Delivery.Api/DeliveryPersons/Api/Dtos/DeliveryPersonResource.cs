namespace Delivery.Api.DeliveryPersons.Api.Dtos;

public record DeliveryPersonResource(
    string Id,
    string Name,
    string Code,
    string Phone,
    IEnumerable<string> AssignedShipments,
    bool IsActive);