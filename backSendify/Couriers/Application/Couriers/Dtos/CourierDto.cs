using backSendify.Shared.Application.Common.Dtos;

namespace backSendify.Couriers.Application.Couriers.Dtos;

public record CourierDto(
    Guid Id,
    string Name,
    ContactDto? Contact,
    PricingDto? Pricing,
    decimal? CostPerKg,
    int? EstimatedDays,
    bool IsActive,
    IReadOnlyCollection<string> Services,
    IReadOnlyCollection<string> Coverage,
    DateTime CreatedAt,
    DateTime UpdatedAt);
