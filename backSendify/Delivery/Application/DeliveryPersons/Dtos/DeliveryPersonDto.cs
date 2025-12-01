namespace backSendify.Delivery.Application.DeliveryPersons.Dtos;

public record DeliveryPersonDto(
    Guid Id,
    string Name,
    string Code,
    string Phone,
    bool IsActive,
    IReadOnlyCollection<Guid> AssignedShipments,
    DateTime CreatedAt,
    DateTime UpdatedAt);
