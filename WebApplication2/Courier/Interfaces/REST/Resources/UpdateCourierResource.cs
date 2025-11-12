namespace WebApplication2.Courier.Interfaces.REST.Resources;
public record UpdateCourierResource(string? Name, decimal? CostPerKg, int? EstimatedDays, bool? IsActive);