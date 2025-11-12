namespace Delivery.Api.DeliveryPersons.Domain.Commands;

public record UpdateDeliveryPersonCommand(
    string Id,
    string? Name,
    string? Phone);