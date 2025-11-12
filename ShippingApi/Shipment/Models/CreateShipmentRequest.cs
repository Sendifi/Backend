using System.Text.Json.Serialization;

namespace ShippingApi.Shipment.Models;

public class CreateShipmentRequest
{
    public required Person Sender { get; set; }
    public required Person Recipient { get; set; }
    public required Address OriginAddress { get; set; }
    public required Address DestinationAddress { get; set; }
    public required double Weight { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ShipmentStatus Status { get; set; } = ShipmentStatus.PENDING;
    
    public int? CourierId { get; set; }
    public string? DeliveryPersonId { get; set; }
}
