using System.ComponentModel.DataAnnotations;

namespace backSendify.Delivery.Application.DeliveryPersons.Requests;

public class DeliveryPersonUpdateRequest
{
    public string? Name { get; set; }
    [Phone]
    public string? Phone { get; set; }
    public bool? IsActive { get; set; }
    public IEnumerable<Guid>? AssignedShipments { get; set; }
}
