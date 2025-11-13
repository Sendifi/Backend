namespace backSendify.Shared.Domain.Common.ValueObjects;

public class PricingInfo
{
    public decimal BaseCost { get; private set; }
    public decimal PerKg { get; private set; }

    private PricingInfo() {}

    public PricingInfo(decimal baseCost, decimal perKg)
    {
        BaseCost = baseCost;
        PerKg = perKg;
    }
}
