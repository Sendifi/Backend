using backSendify.Shipments.Domain.Shipments.Enums;

namespace backSendify.Shipments.Application.Shipments.Requests;

public class ShipmentQueryParameters
{
    public ShipmentStatus? Status { get; set; }
    public string? TrackingCode { get; set; }
    public Guid? DeliveryPersonId { get; set; }
}
