using System.ComponentModel.DataAnnotations;
using backSendify.Shared.Application.Common.Dtos;
using backSendify.Shipments.Domain.Shipments.Enums;

namespace backSendify.Shipments.Application.Shipments.Requests;

public class ShipmentCreateRequest
{
    public string? TrackingCode { get; set; }

    [Required]
    public PersonDto Sender { get; set; } = null!;

    [Required]
    public PersonDto Recipient { get; set; } = null!;

    [Required]
    public AddressDto OriginAddress { get; set; } = null!;

    [Required]
    public AddressDto DestinationAddress { get; set; } = null!;

    [Required]
    [Range(0.01, double.MaxValue)]
    public double Weight { get; set; }

    public decimal? Cost { get; set; }

    [Required]
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;

    public Guid? CourierId { get; set; }
    public Guid? DeliveryPersonId { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}
