using backSendify.Shared.Domain.Common;
using backSendify.Shared.Domain.Common.ValueObjects;
using backSendify.Shipments.Domain.Shipments.Enums;

namespace backSendify.Shipments.Domain.Shipments.Entities;

public class Shipment : AuditableEntity
{
    public string TrackingCode { get; set; } = string.Empty;
    public PersonInfo Sender { get; set; } = null!;
    public PersonInfo Recipient { get; set; } = null!;
    public Address OriginAddress { get; set; } = null!;
    public Address DestinationAddress { get; set; } = null!;
    public double Weight { get; set; }
    public decimal Cost { get; set; }
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    public Guid? CourierId { get; set; }
    public Guid? DeliveryPersonId { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}
