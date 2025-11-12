namespace WebApplication2.Courier.Domain.Model.Aggregates;

using WebApplication2.Courier.Domain.Model.Commands;

public class Courier
{
    protected Courier() { Name = string.Empty; }

    public Courier(CreateCourierCommand cmd)
    {
        if (string.IsNullOrWhiteSpace(cmd.Name)) throw new ArgumentException("name is required");
        if (cmd.CostPerKg is <= 0) throw new ArgumentException("costPerKg must be positive");

        Name = cmd.Name.Trim();
        CostPerKg = cmd.CostPerKg!.Value;
        EstimatedDays = cmd.EstimatedDays ?? 2;
        IsActive = cmd.IsActive ?? true;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal CostPerKg { get; private set; }
    public int EstimatedDays { get; private set; }
    public bool IsActive { get; private set; }

    public void Update(string? name, decimal? costPerKg, int? estimatedDays, bool? isActive)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name!.Trim();
        if (costPerKg is not null && costPerKg <= 0) throw new ArgumentException("costPerKg must be positive");
        if (costPerKg is not null) CostPerKg = costPerKg.Value;
        if (estimatedDays is not null && estimatedDays <= 0) throw new ArgumentException("estimatedDays must be positive");
        if (estimatedDays is not null) EstimatedDays = estimatedDays.Value;
        if (isActive is not null) IsActive = isActive.Value;
    }
}