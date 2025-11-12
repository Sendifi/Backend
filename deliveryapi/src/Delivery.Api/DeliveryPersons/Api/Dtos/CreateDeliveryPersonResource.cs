namespace Delivery.Api.DeliveryPersons.Api.Dtos;

public record CreateDeliveryPersonResource(
    string Name,
    string? Code,
    string Phone);