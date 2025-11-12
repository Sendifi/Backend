namespace WebApplication2.Courier.Interfaces.REST.Resources;
public record CreateCourierResource(string Name, decimal CostPerKg, int? EstimatedDays, bool? IsActive);

