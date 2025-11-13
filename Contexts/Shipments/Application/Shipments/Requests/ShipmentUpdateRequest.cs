using System.ComponentModel.DataAnnotations;
using backSendify.Shared.Application.Common.Dtos;
using backSendify.Shipments.Domain.Shipments.Enums;

namespace backSendify.Shipments.Application.Shipments.Requests;

public class ShipmentUpdateRequest
{
    public PersonDto? Sender { get; set; }
    public PersonDto? Recipient { get; set; }
    public AddressDto? OriginAddress { get; set; }
    public AddressDto? DestinationAddress { get; set; }
    [Range(0.01, double.MaxValue)]
    public double? Weight { get; set; }
    public decimal? Cost { get; set; }
    public ShipmentStatus? Status { get; set; }
    public Guid? CourierId { get; set; }
    public Guid? DeliveryPersonId { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}
