namespace Delivery.Api.DeliveryPersons.Domain.Commands;

public record CreateDeliveryPersonCommand(
    string Name,
    string? Code,
    string Phone);