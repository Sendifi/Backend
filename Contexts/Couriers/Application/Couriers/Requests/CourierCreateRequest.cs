using System.ComponentModel.DataAnnotations;
using backSendify.Shared.Application.Common.Dtos;

namespace backSendify.Couriers.Application.Couriers.Requests;

public class CourierCreateRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public ContactDto? Contact { get; set; }
    public PricingDto? Pricing { get; set; }
    public decimal? CostPerKg { get; set; }
    public int? EstimatedDays { get; set; }
    public bool IsActive { get; set; } = true;
    public IEnumerable<string>? Services { get; set; }
    public IEnumerable<string>? Coverage { get; set; }
}
