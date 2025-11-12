namespace WebApplication2.Courier.Domain.Model.Commands;

public readonly record struct CreateCourierCommand(
    string Name,
    decimal? CostPerKg,
    int? EstimatedDays,
    bool? IsActive
);