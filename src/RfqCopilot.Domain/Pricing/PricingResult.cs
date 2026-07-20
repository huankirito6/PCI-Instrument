namespace RfqCopilot.Domain.Pricing;

/// <summary>
/// Deterministic result for a single quotation line.
/// </summary>
/// <param name="LineTotal">Quantity * SellingPrice.</param>
/// <param name="GrossProfit">SellingPrice - CostPrice (per unit).</param>
/// <param name="GrossProfitPercent">(SellingPrice - CostPrice) / SellingPrice * 100.</param>
public readonly record struct PricingResult(
    decimal LineTotal,
    decimal GrossProfit,
    decimal GrossProfitPercent);
