using System.Text.Json.Serialization;

namespace ShippingApi.Shipment.Models;

public class UpdateShipmentRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ShipmentStatus? Status { get; set; }
    
    public string? DeliveryPersonId { get; set; }
    public string? EstimatedDelivery { get; set; }
    public decimal? Cost { get; set; }
    public int? CourierId { get; set; }
}
