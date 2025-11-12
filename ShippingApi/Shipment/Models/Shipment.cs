using System.Text.Json.Serialization;

namespace ShippingApi.Shipment.Models;

public class Shipment
{
    public required string Id { get; set; }
    public required string TrackingCode { get; set; }
    public required Person Sender { get; set; }
    public required Person Recipient { get; set; }
    public required Address OriginAddress { get; set; }
    public required Address DestinationAddress { get; set; }
    public required double Weight { get; set; }
    public decimal Cost { get; set; } = 0;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ShipmentStatus Status { get; set; }
    
    public int? CourierId { get; set; }
    public string? DeliveryPersonId { get; set; }
    public string? EstimatedDelivery { get; set; }
    public required string CreatedAt { get; set; }
    public required string UpdatedAt { get; set; }
}
