using backSendify.Shared.Application.Common.Dtos;

namespace backSendify.Couriers.Application.Couriers.Requests;

public class CourierUpdateRequest
{
    public string? Name { get; set; }
    public ContactDto? Contact { get; set; }
    public PricingDto? Pricing { get; set; }
    public decimal? CostPerKg { get; set; }
    public int? EstimatedDays { get; set; }
    public bool? IsActive { get; set; }
    public IEnumerable<string>? Services { get; set; }
    public IEnumerable<string>? Coverage { get; set; }
}
