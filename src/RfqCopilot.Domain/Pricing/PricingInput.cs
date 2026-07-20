namespace RfqCopilot.Domain.Pricing;

/// <summary>
/// Inputs for a single quotation line. Cost and selling price are entered/approved
/// by a human; this type carries no automatic pricing decision.
/// </summary>
/// <param name="CostPrice">Vendor cost price (per unit).</param>
/// <param name="SellingPrice">Selling price (per unit), entered manually or derived from a factor.</param>
/// <param name="Quantity">Number of units for this line.</param>
public readonly record struct PricingInput(decimal CostPrice, decimal SellingPrice, int Quantity);
