using backSendify.Shared.Domain.Common;

namespace backSendify.Delivery.Domain.DeliveryPersons.Entities;

public class DeliveryPerson : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public List<Guid> AssignedShipments { get; set; } = new();
}
