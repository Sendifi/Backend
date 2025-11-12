
namespace WebApplication2.Courier.Interfaces.REST.Resources;
public record CourierResource(int Id, string Name, decimal CostPerKg, int EstimatedDays, bool IsActive);